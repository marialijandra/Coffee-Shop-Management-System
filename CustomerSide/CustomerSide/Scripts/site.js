function toggleCartDrawer(open) {
    var drawer = document.getElementById('cartDrawer');
    var overlay = document.getElementById('cartOverlay');
    if (!drawer || !overlay) return;
    drawer.style.display = open ? 'block' : 'none';
    overlay.style.display = open ? 'block' : 'none';
}

function toggleOptionsPopover(btn) {
    var card = btn.closest('.product-card');
    var pop = card.querySelector('.options-popover');
    var wasOpen = pop.classList.contains('open');

    document.querySelectorAll('.options-popover.open').forEach(function (p) {
        p.classList.remove('open');
        p.closest('.product-card').classList.remove('popover-open');
    });

    if (!wasOpen) {
        pop.classList.add('open');
        card.classList.add('popover-open');
    }
}

function selectOption(btn) {
    var group = btn.getAttribute('data-group');
    var row = btn.parentElement;
    row.querySelectorAll('.toggle-btn').forEach(function (b) { b.classList.remove('active'); });
    btn.classList.add('active');

    var popover = btn.closest('.options-popover');
    var hidden = popover.querySelector('.opt-' + group);
    if (hidden) hidden.value = btn.getAttribute('data-value');
}

function changeQty(btn, delta) {
    var row = btn.parentElement;
    var span = row.querySelector('.qty-value');
    var val = parseInt(span.textContent, 10) + delta;
    if (val < 1) val = 1;
    span.textContent = val;

    var popover = btn.closest('.options-popover');
    var hidden = popover.querySelector('.opt-qty');
    if (hidden) hidden.value = val;
}

document.addEventListener('click', function (ev) {
    if (!ev.target.closest('.options-popover') && !ev.target.closest('.add-btn')) {
        document.querySelectorAll('.options-popover.open').forEach(function (p) {
            p.classList.remove('open');
            p.closest('.product-card').classList.remove('popover-open');
        });
    }
});

(function () {
    var lastY = window.scrollY;
    var header = document.getElementById('siteHeader');
    if (!header) return;

    window.addEventListener('scroll', function () {
        var currentY = window.scrollY;

        if (currentY > lastY && currentY > 80) {
            header.classList.add('nav-hidden');
        } else {
            header.classList.remove('nav-hidden');
        }

        lastY = currentY;
    }, { passive: true });
})();

document.addEventListener('DOMContentLoaded', function () {
    var toast = document.querySelector('.cart-toast');
    if (toast) {
        setTimeout(function () {
            toast.style.transition = 'opacity .4s ease';
            toast.style.opacity = '0';
        }, 4000);
    }

    var pills = document.querySelectorAll('.filter-pills a[data-filter]');
    var cards = document.querySelectorAll('.product-card[data-tag]');
    pills.forEach(function (pill) {
        pill.addEventListener('click', function (ev) {
            ev.preventDefault();
            pills.forEach(function (p) { p.classList.remove('active'); });
            pill.classList.add('active');
            var filter = pill.getAttribute('data-filter');
            cards.forEach(function (card) {
                var tag = card.getAttribute('data-tag');
                card.style.display = (filter === 'all' || tag === filter) ? '' : 'none';
            });
        });
    });

    var hero = document.querySelector('.hero');
    if (hero) {
        var glow = document.createElement('div');
        glow.className = 'cursor-glow';
        document.body.appendChild(glow);

        hero.addEventListener('mousemove', function (e) {
            glow.style.left = e.clientX + 'px';
            glow.style.top = e.clientY + 'px';
            glow.classList.add('active');
        });

        hero.addEventListener('mouseleave', function () {
            glow.classList.remove('active');
        });
    }
});