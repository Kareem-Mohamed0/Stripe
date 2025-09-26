// ألوان مقترحة
const colors = ["#FF5733", "#33FF57", "#3357FF", "#FF33A1", "#FFD133", "#33FFF5"];

document.querySelectorAll(".product-item").forEach(card => {
  const randomColor = colors[Math.floor(Math.random() * colors.length)];
  
  // هنا بنغير قيمة الـ variable على مستوى الكارد
  card.style.setProperty("--primary-card-color", randomColor);
});




    // async function checkout() {
    //     try {
    //         const response = await fetch("http://localhost:5168/api/Payments/create-checkout-session", {
    //             method: "POST",
    //             headers: { "Content-Type": "application/json" },
    //             body: JSON.stringify({
    //                 productName: "Apple",
    //                 amount: 20,
    //                 status: "Pending"
    //             })
    //         });

    //         if (!response.ok) {
    //             // لو السيرفر رجّع BadRequest أو أي error
    //             const errorText = await response.text();
    //             console.error("Backend error:", errorText);
    //             alert("❌ Error from server: " + errorText);
    //             return;
    //         }

    //         const data = await response.json();
    //         console.log("Backend response:", data);

    //         if (data.url) {
    //             console.log("Redirecting to:", data.url);
    //             window.location.href = data.url; // روح على صفحة الدفع
    //         } else {
    //             console.error("⚠️ No URL returned from backend:", data);
    //             alert("No checkout URL received.");
    //         }
    //     } catch (err) {
    //         console.error("❌ Exception:", err);
    //         alert("Something went wrong: " + err.message);
    //     }
    // }

    // // اربطها بالزر
    // document.getElementById("checkout-button").addEventListener("click", checkout);

// Prodcusts page
const ProductList = document.querySelector('.product-list');
async function LoadProducts() {
  try{
    const response =await fetch('http://localhost:5168/api/Products');
    if (!response.ok) throw new Error('failed to fetch products');
    const products =await response.json();
    ProductList.innerHTML = '';
    products.forEach(product => {
      ProductList.innerHTML += `
      <div class="product-item">
        <div class="background-white">
          <img src="${product.imageUrl}" alt="${product.name}">
          <h3>${product.name}</h3>
          <p>$${product.price.toFixed(2)}</p>
          <button data-product-id="${product.id}">Add to Cart</button>
        </div>
      </div>
      `;
    });
  }
  catch(error){
    console.error('Error loading products:', error);
  }
}




// flying to cart animation
const cartIcon = document.querySelector('.fa-cart-shopping');
document.querySelectorAll('.product-item button').forEach(button => {
    button.addEventListener('click', (e) =>{
        /* definitions */
        const img = e.target.closest('.product-item').querySelector('img');
        const cloneImg = img.cloneNode(true);
        const cartRect = cartIcon.getBoundingClientRect();
        const imgRect = img.getBoundingClientRect();
        /* end definitions */
        /* add clone to body */
        cloneImg.style.position = 'fixed';
        cloneImg.style.left = imgRect.left + 'px';
        cloneImg.style.top = imgRect.top + 'px';
        cloneImg.style.width = img.width + 'px';
        cloneImg.style.height = img.height + 'px';
        document.body.appendChild(cloneImg);
        /* end add clone to body */
        /* move to cart */

        setTimeout(() => {
            cloneImg.style.zIndex = 1000;
            cloneImg.style.transition = 'all 1s ease-in-out';
            cloneImg.style.left = cartRect.left + 'px';
            cloneImg.style.top = cartRect.top + 'px';
            cloneImg.style.width = '0px';
            cloneImg.style.height = '0px';
            cloneImg.style.opacity = 0.8;
            cloneImg.style.borderRadius = '50%';
        }, 100);
        setTimeout(() => {
            cloneImg.remove();
        }, 900);
        /* end move to cart */
    });
});

const searchBar = document.querySelector('.search-bar');

window.addEventListener('scroll', () => {
  if (window.scrollY > 50) { 
    searchBar.classList.add('scrolled');
  } else {
    searchBar.classList.remove('scrolled');
  }
});


LoadProducts();