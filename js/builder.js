let builderData = {
  size: '0.5kg',
  basePrice: 600,
  servings: 'Serves 4 - 6 people',
  flavor: 'Madagascar Vanilla Bean & Fresh Strawberry',
  flavorExtra: 0,
  frosting: 'Soft Swiss Buttercream',
  toppings: [],
  toppingsPrice: 0,
  message: 'Happy Birthday!',
  isEggless: true
};

function selectBuilderSize(size, basePrice, servings) {
  builderData.size = size;
  builderData.basePrice = basePrice;
  builderData.servings = servings;

  document.querySelectorAll('.builder-size-card').forEach(card => {
    card.classList.remove('border-rose-600', 'bg-rose-50/60', 'ring-2', 'ring-rose-400');
    card.classList.add('border-slate-200');
  });

  const selectedBtn = document.getElementById(`btn-size-${size}`);
  if (selectedBtn) {
    selectedBtn.classList.remove('border-slate-200');
    selectedBtn.classList.add('border-rose-600', 'bg-rose-50/60', 'ring-2', 'ring-rose-400');
  }

  updateBuilderSummary();
}

function updateBuilderFlavor() {
  const select = document.getElementById('builderFlavorSelect');
  if (!select) return;
  const option = select.options[select.selectedIndex];
  builderData.flavor = option.value;
  builderData.flavorExtra = parseInt(option.getAttribute('data-extra') || '0');
  updateBuilderSummary();
}

function updateBuilderFrosting() {
  const radio = document.querySelector('input[name="builderFrosting"]:checked');
  if (radio) {
    builderData.frosting = radio.value;
  }
  updateBuilderSummary();
}

function updateBuilderToppings() {
  let list = [];
  let extraPrice = 0;
  document.querySelectorAll('.builder-topping-chk:checked').forEach(chk => {
    list.push(chk.value);
    extraPrice += parseInt(chk.getAttribute('data-price') || '0');
  });
  builderData.toppings = list;
  builderData.toppingsPrice = extraPrice;
  updateBuilderSummary();
}

function updateBuilderMessage() {
  const msgInput = document.getElementById('builderCustomMessageInput');
  if (msgInput) {
    builderData.message = msgInput.value || 'Happy Birthday!';
  }
  updateBuilderSummary();
}

function updateBuilderSummary() {
  const total = builderData.basePrice + builderData.flavorExtra + builderData.toppingsPrice;

  const elSize = document.getElementById('previewSize');
  const elFlavor = document.getElementById('previewFlavor');
  const elFrosting = document.getElementById('previewFrosting');
  const elToppings = document.getElementById('previewToppings');
  const elMessage = document.getElementById('previewMessage');
  const elTotal = document.getElementById('previewTotalPrice');

  if (elSize) elSize.textContent = `${builderData.size} (${builderData.servings})`;
  if (elFlavor) elFlavor.textContent = builderData.flavor;
  if (elFrosting) elFrosting.textContent = builderData.frosting;
  if (elToppings) elToppings.textContent = builderData.toppings.length ? builderData.toppings.join(', ') : 'Standard Decoration';
  if (elMessage) elMessage.textContent = `"${builderData.message}"`;
  if (elTotal) elTotal.textContent = `₹${total}`;

  return total;
}

function addCustomCakeToCart() {
  const total = updateBuilderSummary();
  const egglessTag = document.getElementById('builderEgglessChk')?.checked ? '100% Eggless' : 'Contains Egg';
  
  const customItem = {
    id: 'custom-cake-' + Date.now(),
    title: `Custom Cake (${builderData.flavor})`,
    unit: `${builderData.size} • ${builderData.frosting} • ${egglessTag}`,
    price: total,
    qty: 1,
    image: 'https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=600&q=80',
    details: `Message: "${builderData.message}" | Toppings: ${builderData.toppings.join(', ') || 'None'}`
  };

  globalCart.addItem(customItem);
  toggleCartDrawer(true);
}

document.addEventListener('DOMContentLoaded', () => {
  updateBuilderSummary();
});
