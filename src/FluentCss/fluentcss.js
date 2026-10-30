(function () {
    var prefix = '/fluentcss';
    var linkId = 'fluentcss';
    var cooldown = 100;

    function refresh() {
        var link = document.getElementById(linkId);
        if (!link) return;

        fetch(prefix + '/hash', { cache: 'no-store' })
            .then(function (response) { return response.ok ? response.text() : null; })
            .then(function (hash) {
                if (!hash) return;
                hash = hash.trim();
                if (hash && hash !== link.getAttribute('data-fluentcss-hash')) {
                    link.setAttribute('data-fluentcss-hash', hash);
                    link.setAttribute('href', prefix + '/' + hash + '.css');
                }
            })
            .catch(function () { });
    }

    function hasClass(node) {
        if (!node || node.nodeType !== 1) return false;
        if (node.classList && node.classList.length) return true;
        return !!node.querySelector('[class]');
    }

    function onMutations(records) {
        for (var i = 0; i < records.length; i++) {
            var added = records[i].addedNodes;
            for (var j = 0; j < added.length; j++) {
                if (hasClass(added[j])) {
                    schedule();
                    return;
                }
            }
        }
    }

    var lastRun = 0;
    var timer = null;

    function schedule() {
        var now = Date.now();
        var elapsed = now - lastRun;

        if (elapsed >= cooldown) {
            lastRun = now;
            refresh();
        } else {
            if (timer) clearTimeout(timer);
            timer = setTimeout(function () {
                lastRun = Date.now();
                refresh();
            }, cooldown - elapsed);
        }
    }

    function start() {
        if (document.body) {
            new MutationObserver(onMutations).observe(document.body, { childList: true, subtree: true });
        }
        refresh();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', start);
    } else {
        start();
    }
})();
