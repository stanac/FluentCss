// Shared browser helpers for the FluentCss documentation, used by both the
// static-SSR and WebAssembly hosts. Loaded as a classic script so the functions
// are available to Blazor JS interop (window.fc*) and to inline onclick handlers.
(function () {
    function applyTheme() {
        try {
            var saved = localStorage.getItem('fluentcss-theme');
            var prefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
            var dark = saved === 'dark' || (!saved && prefersDark);
            document.documentElement.classList.toggle('theme-dark', dark);
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

    window.fcApplyTheme = applyTheme;

    window.fcToggleTheme = function () {
        var root = document.documentElement;
        var dark = root.classList.toggle('theme-dark');
        try {
            localStorage.setItem('fluentcss-theme', dark ? 'dark' : 'light');
        } catch (e) { }
    };

    window.fcSetBodyClasses = function (classes) {
        document.body.className = classes;
    };

    window.fcHighlightElement = function (el) {
        if (window.Prism && el) {
            Prism.highlightElement(el);
        }
    };

    function onReady() {
        applyTheme();

        // Static SSR renders code blocks without JS interop, so highlight everything
        // once the document is parsed (navigation does full page loads).
        highlightAll();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', onReady);
    } else {
        onReady();
    }

    // Re-highlight after enhanced navigation, if the host enables it.
    if (window.Blazor && window.Blazor.addEventListener) {
        window.Blazor.addEventListener('enhancedload', highlightAll);
    }
})();
