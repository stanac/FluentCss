// Client-side full-text search over the prebuilt section index
// (_content/FluentCss.Docs/search-index.json). MiniSearch provides the inverted
// index, prefix matching and fuzzy matching; the custom tokenizer keeps
// camelCase, kebab-case and CSS-ish identifiers searchable. The UI is plain DOM
// so it behaves identically in the static-SSR and WebAssembly hosts; the markup
// and class names come from SearchBox.razor.
(function () {
    'use strict';

    var INDEX_URL = '_content/FluentCss.Docs/search-index.json';
    var MAX_RESULTS = 8;

    var indexPromise = null;
    var debounceTimer = null;
    var activeIndex = -1;

    function $(selector) {
        return document.querySelector(selector);
    }

    // ---------------------------------------------------------------------
    // Tokenizer
    //
    // Emits the whole compound token ("hover:bg-blue-700", "BackgroundColor")
    // plus its separator/camelCase pieces so both "hover:bg-blue-700" and
    // "background" / "color" / "blue" match.
    // ---------------------------------------------------------------------

    function tokenize(text) {
        if (!text) {
            return [];
        }

        var tokens = [];
        var re = /[A-Za-z0-9]+(?:[_:.\-][A-Za-z0-9]+)*/g;
        var match;

        while ((match = re.exec(text)) !== null) {
            var compound = match[0];
            tokens.push(compound);

            var parts = compound.split(/[_:.\-]+/);
            for (var i = 0; i < parts.length; i++) {
                pushCamel(parts[i], tokens);
            }
        }

        return tokens;
    }

    function pushCamel(part, out) {
        if (!part) {
            return;
        }

        var pieces = part.split(/(?=[A-Z])/);
        for (var i = 0; i < pieces.length; i++) {
            if (pieces[i]) {
                out.push(pieces[i]);
            }
        }
    }

    // ---------------------------------------------------------------------
    // Index
    // ---------------------------------------------------------------------

    function buildIndex(data) {
        var docs = [];

        (data.pages || []).forEach(function (page) {
            (page.sections || []).forEach(function (section) {
                docs.push({
                    id: page.route + '#' + section.id,
                    route: page.route,
                    anchor: section.id,
                    pageTitle: page.title,
                    group: page.section,
                    heading: section.heading,
                    level: section.level,
                    body: section.text || '',
                    code: section.code || ''
                });
            });
        });

        var mini = new MiniSearch({
            idField: 'id',
            fields: ['pageTitle', 'heading', 'body', 'code'],
            storeFields: ['route', 'anchor', 'pageTitle', 'group', 'heading', 'level', 'body', 'code'],
            tokenize: tokenize,
            searchOptions: {
                prefix: true,
                fuzzy: 0.2,
                boost: { pageTitle: 3, heading: 2.5, body: 1, code: 0.8 },
                combineWith: 'AND'
            }
        });

        mini.addAll(docs);
        return mini;
    }

    function loadIndex() {
        if (indexPromise) {
            return indexPromise;
        }

        if (!window.MiniSearch) {
            indexPromise = Promise.resolve(null);
            return indexPromise;
        }

        indexPromise = fetch(INDEX_URL, { credentials: 'same-origin' })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('HTTP ' + response.status);
                }

                return response.json();
            })
            .then(buildIndex)
            .catch(function (error) {
                console.warn('FluentCss search: could not load the index.', error);
                return null;
            });

        return indexPromise;
    }

    // ---------------------------------------------------------------------
    // Rendering
    // ---------------------------------------------------------------------

    function readClasses(list) {
        return {
            result: list.getAttribute('data-fc-result-class') || '',
            active: list.getAttribute('data-fc-result-active-class') || '',
            path: list.getAttribute('data-fc-result-path-class') || '',
            title: list.getAttribute('data-fc-result-title-class') || '',
            text: list.getAttribute('data-fc-result-text-class') || '',
            mark: list.getAttribute('data-fc-mark-class') || ''
        };
    }

    function escapeHtml(value) {
        return String(value).replace(/[&<>"']/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
        });
    }

    function escapeRegExp(value) {
        return value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    }

    function highlight(text, terms, markClass) {
        var safe = escapeHtml(text);
        var usable = (terms || []).filter(Boolean);

        if (!usable.length) {
            return safe;
        }

        var pattern = usable.map(escapeRegExp).join('|');
        if (!pattern) {
            return safe;
        }

        return safe.replace(new RegExp('(' + pattern + ')', 'ig'),
            '<mark class="' + markClass + '">$1</mark>');
    }

    function makeSnippet(text, terms) {
        if (!text) {
            return '';
        }

        var lower = text.toLowerCase();
        var index = -1;

        (terms || []).forEach(function (term) {
            if (!term) {
                return;
            }

            var found = lower.indexOf(term.toLowerCase());
            if (found >= 0 && (index < 0 || found < index)) {
                index = found;
            }
        });

        if (index < 0) {
            index = 0;
        }

        var start = Math.max(0, index - 60);
        var end = Math.min(text.length, start + 180);
        return (start > 0 ? '\u2026' : '') + text.slice(start, end) + (end < text.length ? '\u2026' : '');
    }

    function renderResults() {
        var input = $('[data-fc-search-input]');
        var list = $('[data-fc-search-results]');
        var status = $('[data-fc-search-status]');
        var empty = $('[data-fc-search-empty]');
        if (!input || !list) {
            return;
        }

        var query = (input.value || '').trim();
        activeIndex = -1;

        if (!query) {
            list.innerHTML = '';
            if (status) {
                status.textContent = '';
            }
            if (empty) {
                empty.hidden = true;
            }
            return;
        }

        loadIndex().then(function (mini) {
            // A newer keystroke may have replaced the query while we loaded.
            if ((input.value || '').trim() !== query) {
                return;
            }

            if (!mini) {
                if (status) {
                    status.textContent = 'Search is unavailable.';
                }
                return;
            }

            var options = {
                prefix: true,
                fuzzy: 0.2,
                boost: { pageTitle: 3, heading: 2.5, body: 1, code: 0.8 },
                combineWith: 'AND'
            };

            var results = mini.search(query, options);
            if (!results.length) {
                options.combineWith = 'OR';
                results = mini.search(query, options);
            }

            if (status) {
                status.textContent = results.length === 1 ? '1 result' : results.length + ' results';
            }

            if (empty) {
                empty.hidden = results.length > 0;
            }

            renderList(list, results.slice(0, MAX_RESULTS), readClasses(list));
        });
    }

    function renderList(list, results, classes) {
        list.innerHTML = '';

        results.forEach(function (result) {
            var headingTerms = (result.match && result.match.heading) || result.queryTerms || [];
            var codeMatched = result.match && result.match.code;
            var bodyTerms = (result.match && result.match.body) || codeMatched || result.queryTerms || [];

            var item = document.createElement('li');
            item.setAttribute('role', 'option');
            item.setAttribute('aria-selected', 'false');

            var link = document.createElement('a');
            link.className = classes.result;
            link.href = result.route + '#' + encodeURIComponent(result.anchor || '');
            link.setAttribute('data-fc-search-result', '');

            var path = document.createElement('span');
            path.className = classes.path;
            path.textContent = result.pageTitle;

            var title = document.createElement('span');
            title.className = classes.title;
            title.innerHTML = highlight(result.heading, headingTerms, classes.mark);

            var snippet = document.createElement('span');
            snippet.className = classes.text;
            snippet.innerHTML = highlight(
                makeSnippet(result.body || result.code || '', bodyTerms), bodyTerms, classes.mark);

            link.appendChild(path);
            link.appendChild(title);
            link.appendChild(snippet);
            item.appendChild(link);
            list.appendChild(item);
        });
    }

    // ---------------------------------------------------------------------
    // Open / close / keyboard
    // ---------------------------------------------------------------------

    function openSearch() {
        var overlay = $('[data-fc-search-overlay]');
        var input = $('[data-fc-search-input]');
        if (!overlay || !input) {
            return;
        }

        overlay.hidden = false;
        loadIndex();
        input.focus();
        input.select();
        renderResults();
    }

    function closeSearch() {
        var overlay = $('[data-fc-search-overlay]');
        if (overlay) {
            overlay.hidden = true;
        }
        activeIndex = -1;
    }

    function isOpen() {
        var overlay = $('[data-fc-search-overlay]');
        return !!overlay && !overlay.hidden;
    }

    function resultLinks() {
        var list = $('[data-fc-search-results]');
        return list ? list.querySelectorAll('[data-fc-search-result]') : [];
    }

    function setActive(next) {
        var list = $('[data-fc-search-results]');
        var links = resultLinks();
        if (!list || !links.length) {
            return;
        }

        var base = list.getAttribute('data-fc-result-class') || '';
        var active = list.getAttribute('data-fc-result-active-class') || '';

        activeIndex = (next + links.length) % links.length;

        for (var i = 0; i < links.length; i++) {
            links[i].className = i === activeIndex ? (base + ' ' + active) : base;

            var option = links[i].closest('li');
            if (option) {
                option.setAttribute('aria-selected', i === activeIndex ? 'true' : 'false');
            }
        }

        links[activeIndex].scrollIntoView({ block: 'nearest' });
    }

    function onKeyDown(event) {
        if ((event.ctrlKey || event.metaKey) && (event.key === 'k' || event.key === 'K')) {
            event.preventDefault();
            openSearch();
            return;
        }

        if (!isOpen()) {
            return;
        }

        if (event.key === 'Escape') {
            event.preventDefault();
            closeSearch();
            return;
        }

        if (event.key === 'ArrowDown') {
            event.preventDefault();
            setActive(activeIndex + 1);
            return;
        }

        if (event.key === 'ArrowUp') {
            event.preventDefault();
            setActive(activeIndex - 1);
            return;
        }

        if (event.key === 'Enter') {
            var links = resultLinks();
            if (links.length) {
                event.preventDefault();
                links[activeIndex >= 0 ? activeIndex : 0].click();
            }
        }
    }

    function onClick(event) {
        var target = event.target;
        if (!target || !target.closest) {
            return;
        }

        if (target.closest('[data-fc-search-open]')) {
            event.preventDefault();
            openSearch();
            return;
        }

        if (target.hasAttribute && target.hasAttribute('data-fc-search-overlay')) {
            closeSearch();
            return;
        }

        if (target.closest('[data-fc-search-result]')) {
            closeSearch();
        }
    }

    function onInput(event) {
        var target = event.target;
        if (!target || !target.hasAttribute || !target.hasAttribute('data-fc-search-input')) {
            return;
        }

        window.clearTimeout(debounceTimer);
        debounceTimer = window.setTimeout(renderResults, 100);
    }

    // ---------------------------------------------------------------------
    // Boot
    // ---------------------------------------------------------------------

    function updateShortcutHints() {
        var mac = /Mac|iPhone|iPad|iPod/.test(navigator.platform || navigator.userAgent || '');
        var hint = mac ? '\u2318K' : 'Ctrl K';

        document.querySelectorAll('[data-fc-search-kbd]').forEach(function (element) {
            element.textContent = hint;
        });
    }

    function boot() {
        updateShortcutHints();

        document.addEventListener('click', onClick);
        document.addEventListener('input', onInput);
        document.addEventListener('keydown', onKeyDown);

        // Enhanced navigation (static SSR) replaces the layout markup; refresh the
        // platform hint and make sure a stale palette is not left open.
        if (window.Blazor && window.Blazor.addEventListener) {
            window.Blazor.addEventListener('enhancedload', function () {
                updateShortcutHints();
                closeSearch();
            });
        }
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', boot);
    } else {
        boot();
    }
})();
