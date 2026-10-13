// Shared browser helpers for the FluentCss documentation, used by both the
// static-SSR and WebAssembly hosts. Loaded as a classic script so the functions
// are available to Blazor JS interop (window.fc*) and to inline onclick handlers.
(function () {
    function readCookie(name) {
        var match = new RegExp('(?:^|; )' + name + '=([^;]*)').exec(document.cookie);
        return match ? decodeURIComponent(match[1]) : null;
    }

    function writeTheme(theme) {
        try {
            document.cookie = 'fluentcss-theme=' + theme + ';path=/;max-age=31536000;samesite=lax';
            localStorage.setItem('fluentcss-theme', theme);
        } catch (e) { }
    }

    function applyTheme() {
        try {
            var saved = readCookie('fluentcss-theme') || localStorage.getItem('fluentcss-theme');
            var prefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
            var theme = saved || (prefersDark ? 'dark' : 'light');
            document.documentElement.classList.toggle('theme-dark', theme === 'dark');

            // Backfill the cookie so the server can render the theme class on the
            // next full page load and the theming never flashes light again.
            if (!readCookie('fluentcss-theme')) {
                writeTheme(theme);
            }
        } catch (e) { }
    }

    function highlightAll() {
        if (!window.Prism) {
            return;
        }

        try {
            Prism.highlightAll();
        } catch (e) { }
    }

    // ---------------------------------------------------------------------
    // Custom theme playground
    //
    // Custom themes are page-scoped: they are applied to <html> at runtime and are
    // deliberately NOT written to the cookie or localStorage. Anything that starts a
    // navigation clears the class and re-applies the persisted light/dark choice, so
    // the custom theme can never leak to another page.
    // ---------------------------------------------------------------------

    var customThemeClass = null;

    function clearCustomTheme() {
        if (!customThemeClass) {
            return;
        }

        document.documentElement.classList.remove(customThemeClass);
        customThemeClass = null;

        // Restore the persisted light/dark theme that the custom theme temporarily
        // replaced on <html>.
        applyTheme();
    }

    function setCustomTheme(themeClass) {
        clearCustomTheme();

        if (!themeClass) {
            return;
        }

        var root = document.documentElement;

        // A custom theme owns the whole ancestor class set: drop the built-in dark
        // class so the two never fight over the same elements.
        root.classList.remove('theme-dark');
        root.classList.add(themeClass);
        customThemeClass = themeClass;
    }

    window.fcSetCustomTheme = setCustomTheme;
    window.fcClearCustomTheme = clearCustomTheme;

    window.fcApplyTheme = applyTheme;

    window.fcToggleTheme = function () {
        var root = document.documentElement;
        var dark = root.classList.toggle('theme-dark');
        writeTheme(dark ? 'dark' : 'light');
    };

    window.fcSetBodyClasses = function (classes) {
        document.body.className = classes;
    };

    window.fcHighlightElement = function (el) {
        if (window.Prism && el) {
            Prism.highlightElement(el);
        }
    };

    // ---------------------------------------------------------------------
    // "On this page" navigation
    //
    // The list is built from the headings that are actually rendered, so no page has
    // to declare it. It is rebuilt when the article DOM changes, which covers Blazor
    // enhanced navigation (static SSR) and client-side routing (WebAssembly).
    // ---------------------------------------------------------------------

    var tocState = null;
    var tocSignature = null;
    var tocBuilding = false;
    var tocTimer = null;

    function slugify(text) {
        return (text || '').toLowerCase().trim()
            .replace(/[^\w\s-]/g, '')
            .replace(/[\s_]+/g, '-')
            .replace(/^-+|-+$/g, '');
    }

    function buildToc() {
        var container = document.getElementById('fc-toc');
        if (!container) {
            return;
        }

        var nav = container.querySelector('[data-fc-toc-nav]');
        if (!nav) {
            return;
        }

        var article = document.querySelector('[data-fc-toc-source]');
        var path = location.pathname.replace(/\/+$/, '') || '/';
        var entries = [];

        // The home page is the only page without the panel.
        if (article && path !== '/') {
            var headings = article.querySelectorAll('h1, h2, h3, h4, h5, h6');
            var used = {};

            for (var i = 0; i < headings.length; i++) {
                var heading = headings[i];
                var text = (heading.textContent || '').replace(/\s+/g, ' ').trim();
                if (!text) {
                    continue;
                }

                var id = heading.id;
                if (!id) {
                    var slug = slugify(text) || ('section-' + (i + 1));
                    var count = used[slug] = (used[slug] || 0) + 1;
                    id = count === 1 ? slug : slug + '-' + count;
                    heading.id = id;
                }

                entries.push({
                    id: id,
                    text: text,
                    level: parseInt(heading.tagName.charAt(1), 10)
                });
            }
        }

        var signature = entries.map(function (e) { return e.level + ':' + e.id; }).join('|');
        if (signature === tocSignature) {
            updateTocActive();
            return;
        }

        tocSignature = signature;
        tocBuilding = true;

        try {
            nav.innerHTML = '';

            if (entries.length < 2) {
                tocState = null;
                container.hidden = true;
            } else {
                var linkClass = nav.getAttribute('data-fc-link-class') || '';
                var min = entries[0].level;

                for (var j = 1; j < entries.length; j++) {
                    if (entries[j].level < min) {
                        min = entries[j].level;
                    }
                }

                for (var k = 0; k < entries.length; k++) {
                    var entry = entries[k];
                    var link = document.createElement('a');
                    link.href = location.pathname + location.search + '#' + entry.id;
                    link.textContent = entry.text;
                    link.className = linkClass;
                    link.setAttribute('data-fc-toc-link', '');
                    link.setAttribute('data-target', entry.id);
                    link.style.paddingLeft = ((entry.level - min) * 12) + 'px';
                    nav.appendChild(link);
                }

                tocState = {
                    links: nav.querySelectorAll('[data-fc-toc-link]'),
                    linkClass: linkClass,
                    activeClass: nav.getAttribute('data-fc-active-class') || linkClass
                };
                container.hidden = false;
            }
        } finally {
            // Ignore the mutations this build just caused.
            window.setTimeout(function () { tocBuilding = false; }, 0);
        }

        updateTocActive();
    }

    function updateTocActive() {
        if (!tocState || !tocState.links.length) {
            return;
        }

        var links = tocState.links;
        var current = null;
        var threshold = 120;

        for (var i = 0; i < links.length; i++) {
            var target = document.getElementById(links[i].getAttribute('data-target'));
            if (target && target.getBoundingClientRect().top <= threshold) {
                current = links[i];
            }
        }

        for (var j = 0; j < links.length; j++) {
            links[j].className = tocState.linkClass;
        }

        if (current) {
            current.className = tocState.activeClass;
        }
    }

    function onTocClick(e) {
        var link = e.target && e.target.closest ? e.target.closest('[data-fc-toc-link]') : null;
        if (!link) {
            return;
        }

        var target = document.getElementById(link.getAttribute('data-target'));
        if (!target) {
            return;
        }

        // Scroll ourselves so Blazor's enhanced navigation does not treat the in-page
        // anchor as a navigation and reset the page to the top.
        e.preventDefault();
        if (e.stopPropagation) {
            e.stopPropagation();
        }

        target.scrollIntoView({ behavior: 'smooth', block: 'start' });

        if (window.history && window.history.replaceState) {
            var url = new URL(location.href);
            url.hash = target.id;
            window.history.replaceState(null, '', url.href);
        }
    }

    function startToc() {
        window.addEventListener('scroll', updateTocActive, { passive: true });

        var nav = document.querySelector('[data-fc-toc-nav]');
        if (nav) {
            nav.addEventListener('click', onTocClick);
        }

        if (!window.MutationObserver || !document.body) {
            return;
        }

        new MutationObserver(function () {
            if (tocBuilding) {
                return;
            }

            window.clearTimeout(tocTimer);
            tocTimer = window.setTimeout(buildToc, 150);
        }).observe(document.body, { childList: true, subtree: true });
    }

    function onReady() {
        // Static SSR renders code blocks without JS interop, so highlight everything
        // once the document is parsed.
        highlightAll();
        buildToc();
        startToc();

        // Re-run after enhanced navigation. Blazor loads after docs.js (the script is
        // at the end of the body), so the listener is attached here.
        if (window.Blazor && window.Blazor.addEventListener) {
            window.Blazor.addEventListener('enhancedload', function () {
                highlightAll();
                buildToc();

                // Enhanced navigation swaps the page without a reload; the custom
                // theme belongs to the previous page and must not survive it.
                clearCustomTheme();
            });
        }

        // Client-side (WebAssembly) routing does not fire enhancedload. Clear the
        // custom theme as soon as the user starts navigating away from the page.
        document.addEventListener('click', function (e) {
            var link = e.target && e.target.closest ? e.target.closest('a[href]') : null;

            if (!link || link.hasAttribute('data-fc-toc-link')) {
                return;
            }

            if (link.target && link.target !== '_self') {
                return;
            }

            var href = link.getAttribute('href') || '';

            // In-page anchors and same-page links are not navigations.
            if (href.charAt(0) === '#' || link.hasAttribute('download')) {
                return;
            }

            clearCustomTheme();
        });

        window.addEventListener('popstate', clearCustomTheme);
    }

    // Apply the theme immediately, while the head is still parsing, so it is set
    // before the first paint. The server renders the same class from the cookie,
    // this only covers first visits and hosts that cannot read the cookie.
    applyTheme();

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', onReady);
    } else {
        onReady();
    }
})();
