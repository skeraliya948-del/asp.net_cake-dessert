// Shared Global JavaScript for Sweet Delights ASP.NET Bakery

class CartManager {
  constructor() {
    this.cart = JSON.parse(localStorage.getItem('sweet_delights_cart')) || [];
    this.listeners = [];
  }

  save() {
    localStorage.setItem('sweet_delights_cart', JSON.stringify(this.cart));
    this.notify();
  }

  subscribe(callback) {
    this.listeners.push(callback);
  }

  notify() {
    this.listeners.forEach(cb => cb(this.cart));
    this.updateBadges();
  }

  addItem(product) {
    const existing = this.cart.find(item => item.id === product.id);
    if (existing) {
      existing.qty += (product.qty || 1);
    } else {
      this.cart.push({ ...product, qty: product.qty || 1 });
    }
    this.save();
    this.showToast(`Added "${product.title}" to cart! 🍰`);
  }

  updateQty(id, change) {
    const item = this.cart.find(i => i.id === id);
    if (item) {
      item.qty += change;
      if (item.qty <= 0) {
        this.cart = this.cart.filter(i => i.id !== id);
      }
      this.save();
    }
  }

  clear() {
    this.cart = [];
    this.save();
  }

  getTotalCount() {
    return this.cart.reduce((acc, item) => acc + item.qty, 0);
  }

  getSubtotal() {
    return this.cart.reduce((acc, item) => acc + (item.price * item.qty), 0);
  }

  updateBadges() {
    const count = this.getTotalCount();
    document.querySelectorAll('.cart-badge-count').forEach(el => {
      el.textContent = count;
      if (count > 0) {
        el.classList.remove('hidden');
      } else {
        el.classList.add('hidden');
      }
    });
  }

  showToast(message) {
    let container = document.getElementById('toast-container');
    if (!container) {
      container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'fixed bottom-5 right-5 z-50 flex flex-col gap-2 pointer-events-none';
      document.body.appendChild(container);
    }

    const toast = document.createElement('div');
    toast.className = 'bg-slate-900 text-white text-xs font-semibold px-4 py-3 rounded-2xl shadow-xl flex items-center gap-2 transform translate-y-4 opacity-0 transition duration-300 pointer-events-auto border border-rose-500/30';
    toast.innerHTML = `<span class="text-rose-400">✨</span> ${message}`;
    
    container.appendChild(toast);

    requestAnimationFrame(() => {
      toast.classList.remove('translate-y-4', 'opacity-0');
    });

    setTimeout(() => {
      toast.classList.add('opacity-0', 'translate-y-2');
      setTimeout(() => toast.remove(), 300);
    }, 2800);
  }
}

const globalCart = new CartManager();

function toggleCartDrawer(open) {
  let drawer = document.getElementById('globalCartDrawer');
  if (!drawer) return;

  if (open === true) {
    drawer.classList.remove('hidden');
  } else if (open === false) {
    drawer.classList.add('hidden');
  } else {
    drawer.classList.toggle('hidden');
  }
  renderCartDrawerContents();
}

function renderCartDrawerContents() {
  const container = document.getElementById('drawerCartItems');
  const subtotalEl = document.getElementById('drawerSubtotal');
  if (!container) return;

  if (globalCart.cart.length === 0) {
    container.innerHTML = `
      <div class="text-center py-16 text-slate-400">
        <i class="fa-solid fa-basket-shopping text-5xl mb-3 text-rose-200"></i>
        <p class="text-sm font-semibold text-slate-700">Your basket is empty</p>
        <p class="text-xs text-slate-400 mt-1">Explore our delicious cakes and add your favorites!</p>
        <a href="Menu.aspx" class="inline-block mt-4 bg-rose-600 text-white text-xs font-bold px-4 py-2 rounded-full shadow hover:bg-rose-700 transition">View Menu</a>
      </div>
    `;
  } else {
    container.innerHTML = globalCart.cart.map(item => `
      <div class="flex items-center gap-3 p-3 bg-slate-50 border border-slate-200/80 rounded-2xl">
        <img src="${item.image}" alt="${item.title}" class="w-14 h-14 rounded-xl object-cover border border-rose-100">
        <div class="flex-grow">
          <h4 class="font-bold text-xs text-slate-800 leading-snug">${item.title}</h4>
          <span class="text-[10px] text-slate-500 font-medium">${item.unit || ''}</span>
          <div class="text-xs font-bold text-rose-600 mt-0.5">₹${item.price * item.qty}</div>
        </div>
        <div class="flex items-center gap-1.5 bg-white border border-slate-200 rounded-lg px-2 py-1">
          <button type="button" onclick="globalCart.updateQty('${item.id}', -1); renderCartDrawerContents();" class="text-slate-500 hover:text-rose-600 text-xs font-bold w-5 h-5 flex items-center justify-center">-</button>
          <span class="text-xs font-bold text-slate-800 w-4 text-center">${item.qty}</span>
          <button type="button" onclick="globalCart.updateQty('${item.id}', 1); renderCartDrawerContents();" class="text-slate-500 hover:text-rose-600 text-xs font-bold w-5 h-5 flex items-center justify-center">+</button>
        </div>
      </div>
    `).join('');
  }

  const subtotal = globalCart.getSubtotal();
  if (subtotalEl) subtotalEl.textContent = `₹${subtotal}`;
}

globalCart.subscribe(() => {
  renderCartDrawerContents();
});

function toggleMobileMenu() {
  const menu = document.getElementById('mobileNavMenu');
  if (menu) {
    menu.classList.toggle('hidden');
  }
}

document.addEventListener('DOMContentLoaded', () => {
  globalCart.updateBadges();

  const currentPath = window.location.pathname.split('/').pop() || 'Default.aspx';
  document.querySelectorAll('.nav-link').forEach(link => {
    const href = link.getAttribute('href');
    if (href === currentPath || (currentPath === '' && href === 'Default.aspx')) {
      link.classList.add('text-rose-600', 'font-bold');
      link.classList.remove('text-slate-700');
    }
  });
});
