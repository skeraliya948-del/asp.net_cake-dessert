// Dessert Menu Product Dataset & Filtering Logic for ASP.NET Web Forms

const BAKERY_PRODUCTS = [
  {
    id: "1",
    title: "Royal Belgian Chocolate Truffle Cake",
    category: "cakes",
    isEggless: true,
    price: 650,
    unit: "0.5 kg",
    rating: 4.9,
    reviewsCount: 142,
    badge: "Bestseller 👑",
    image: "https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=600&q=80",
    desc: "Rich Belgian dark chocolate sponge layered with smooth ganache, cocoa dust & gold leaf flakes."
  },
  {
    id: "2",
    title: "Velvety Red Velvet Cream Cheese Cake",
    category: "cakes",
    isEggless: true,
    price: 700,
    unit: "0.5 kg",
    rating: 4.8,
    reviewsCount: 98,
    badge: "Trending 🔥",
    image: "https://images.unsplash.com/photo-1586985289688-ca3cf47d3e6e?auto=format&fit=crop&w=600&q=80",
    desc: "Crimson cocoa sponge infused with real vanilla bean and iced with silky cream cheese frosting."
  },
  {
    id: "3",
    title: "Parisian Artisanal Macaron Box (6 Pcs)",
    category: "macarons",
    isEggless: false,
    price: 480,
    unit: "Box of 6",
    rating: 4.9,
    reviewsCount: 110,
    badge: "Artisanal 🇫🇷",
    image: "https://images.unsplash.com/photo-1569864358642-9d1684040f43?auto=format&fit=crop&w=600&q=80",
    desc: "Assorted delicate french macarons: Pistachio blush, Strawberry cream, Lemon curd, and Dark Chocolate."
  },
  {
    id: "4",
    title: "Fresh Vanilla Berry Celebration Cake",
    category: "cakes",
    isEggless: true,
    price: 850,
    unit: "1.0 kg",
    rating: 5.0,
    reviewsCount: 215,
    badge: "Chef Special 🌟",
    image: "https://images.unsplash.com/photo-1535141192574-5d4897c13136?auto=format&fit=crop&w=600&q=80",
    desc: "Fluffy Madagascar vanilla sponge layered with fresh strawberries, blueberries and whipped cream."
  },
  {
    id: "5",
    title: "New York Strawberry Cheesecake Slice",
    category: "pastries",
    isEggless: true,
    price: 240,
    unit: "Single Slice",
    rating: 4.9,
    reviewsCount: 84,
    badge: "Popular",
    image: "https://images.unsplash.com/photo-1533134242443-d4fd215305ad?auto=format&fit=crop&w=600&q=80",
    desc: "Classic dense & creamy New York cheesecake baked over a buttery graham cracker crust with berry glaze."
  }
];

let activeCategory = 'all';
let activeSort = 'featured';

function renderMenuGrid() {
  const container = document.getElementById('menuProductGrid');
  if (!container) return;

  const searchVal = (document.getElementById('menuSearchInput')?.value || '').toLowerCase();
  const egglessOnly = document.getElementById('egglessFilterToggle')?.checked || false;

  let filtered = BAKERY_PRODUCTS.filter(item => {
    const matchCat = (activeCategory === 'all') || (item.category === activeCategory);
    const matchSearch = item.title.toLowerCase().includes(searchVal) || item.desc.toLowerCase().includes(searchVal);
    const matchEggless = !egglessOnly || item.isEggless;
    return matchCat && matchSearch && matchEggless;
  });

  if (activeSort === 'price-low') {
    filtered.sort((a, b) => a.price - b.price);
  } else if (activeSort === 'price-high') {
    filtered.sort((a, b) => b.price - a.price);
  } else if (activeSort === 'rating') {
    filtered.sort((a, b) => b.rating - a.rating);
  }

  if (filtered.length === 0) {
    container.innerHTML = `
      <div class="col-span-full text-center py-16 bg-rose-50/50 rounded-3xl border border-rose-100">
        <i class="fa-solid fa-cookie-bite text-5xl text-rose-300 mb-3"></i>
        <h3 class="font-serif-heading text-lg font-bold text-slate-800">No matching desserts found</h3>
        <p class="text-xs text-slate-500 mt-1">Try resetting your filter or searching for another keyword.</p>
      </div>
    `;
    return;
  }

  container.innerHTML = filtered.map(item => `
    <div class="cake-card bg-white rounded-3xl overflow-hidden border border-rose-100/80 shadow-sm flex flex-col group">
      <div class="relative overflow-hidden h-52">
        <img src="${item.image}" alt="${item.title}" class="w-full h-full object-cover group-hover:scale-105 transition duration-500">
        
        <div class="absolute top-3 left-3 flex flex-col gap-1">
          ${item.badge ? `<span class="bg-rose-600 text-white text-[10px] font-bold px-2.5 py-1 rounded-full shadow-md">${item.badge}</span>` : ''}
          ${item.isEggless ? `<span class="bg-emerald-600 text-white text-[10px] font-bold px-2.5 py-1 rounded-full shadow-md flex items-center gap-1">🌱 Eggless</span>` : ''}
        </div>

        <span class="absolute top-3 right-3 bg-white/95 backdrop-blur-md text-slate-800 text-[11px] font-bold px-2.5 py-1 rounded-full shadow-md flex items-center gap-1">
          <i class="fa-solid fa-star text-amber-400 text-xs"></i> ${item.rating}
        </span>
      </div>

      <div class="p-5 flex flex-col flex-grow justify-between space-y-3">
        <div>
          <span class="text-[10px] font-bold uppercase tracking-wider text-rose-500">${item.unit}</span>
          <h3 class="font-serif-heading font-bold text-slate-900 text-base leading-snug group-hover:text-rose-600 transition">${item.title}</h3>
          <p class="text-slate-500 text-xs mt-1.5 leading-relaxed line-clamp-2">${item.desc}</p>
        </div>

        <div class="space-y-2 pt-3 border-t border-rose-50">
          <div class="flex items-center justify-between">
            <div>
              <span class="text-[10px] text-slate-400 block font-semibold uppercase">Price</span>
              <span class="font-serif-heading font-bold text-rose-600 text-xl">₹${item.price}</span>
            </div>

            <a href="ViewDetails.aspx?pid=${item.id}" class="bg-slate-100 hover:bg-slate-200 text-slate-700 px-3 py-2 rounded-xl text-xs font-bold transition flex items-center gap-1">
              <i class="fa-solid fa-eye text-xs text-rose-500"></i> View Details
            </a>
          </div>

          <button type="button" onclick="globalCart.addItem({ id: '${item.id}', title: '${item.title}', price: ${item.price}, image: '${item.image}', unit: '${item.unit}' })" class="w-full bg-rose-50 hover:bg-rose-600 text-rose-700 hover:text-white py-2 rounded-xl text-xs font-bold transition flex items-center justify-center gap-1.5 shadow-sm">
            <i class="fa-solid fa-plus text-xs"></i> Add to Cart
          </button>
        </div>
      </div>
    </div>
  `).join('');
}

function setMenuCategory(cat, element) {
  activeCategory = cat;
  document.querySelectorAll('.menu-cat-btn').forEach(btn => {
    btn.classList.remove('bg-rose-600', 'text-white', 'shadow-md');
    btn.classList.add('bg-slate-100', 'text-slate-700');
  });
  if (element) {
    element.classList.remove('bg-slate-100', 'text-slate-700');
    element.classList.add('bg-rose-600', 'text-white', 'shadow-md');
  }
  renderMenuGrid();
}

function handleSortChange(e) {
  activeSort = e.target.value;
  renderMenuGrid();
}

document.addEventListener('DOMContentLoaded', () => {
  renderMenuGrid();
});
