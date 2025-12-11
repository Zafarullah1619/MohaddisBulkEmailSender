/**
 * Helper script for parent page to auto-resize iframe
 * Include this script on the page where you embed the subscribe form
 * 
 * Usage:
 * <script src="https://subscribe.mohaddis.com/Subscribe/IframeHelper.js"></script>
 * <iframe id="subscribe-iframe" src="https://subscribe.mohaddis.com/Subscribe/form/36" width="600" height="400"></iframe>
 */

(function() {
    'use strict';

    // Listen for resize messages from iframe
    window.addEventListener('message', function(event) {
        // Verify origin for security (optional but recommended)
        // if (event.origin !== 'https://subscribe.mohaddis.com') return;

        if (event.data && event.data.type === 'resize-iframe' && event.data.source === 'subscribe-form') {
            var iframes = document.querySelectorAll('iframe[src*="/Subscribe/form/"]');
            iframes.forEach(function(iframe) {
                if (iframe.contentWindow === event.source) {
                    iframe.style.height = event.data.height + 'px';
                }
            });
        }
    });

    // Fallback: Auto-detect and resize all subscribe iframes
    function autoResizeIframes() {
        var iframes = document.querySelectorAll('iframe[src*="/Subscribe/form/"]');
        iframes.forEach(function(iframe) {
            try {
                if (iframe.contentWindow && iframe.contentWindow.document) {
                    var height = iframe.contentWindow.document.body.scrollHeight;
                    if (height > 0) {
                        iframe.style.height = height + 'px';
                    }
                }
            } catch (e) {
                // Cross-origin iframe - use postMessage method instead
            }
        });
    }

    // Auto-resize on load
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', autoResizeIframes);
    } else {
        autoResizeIframes();
    }

    // Periodic check for dynamic content
    setInterval(autoResizeIframes, 2000);
})();
