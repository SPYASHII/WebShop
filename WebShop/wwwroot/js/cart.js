
function updateBuySubmitState() {
    const buySubmit = document.getElementById('buy-submit');

    const enableValue = 'Buy';
    const disableValue = 'Cart is empty';

    EnableDisableCartSubmit(buySubmit, enableValue, disableValue);
}

function updateClearCartSubmitState() {
    const clearSubmit = document.getElementById('clear-cart-submit');

    const enableValue = "Clear";
    const disableValue = "Cleared";


    EnableDisableCartSubmit(clearSubmit, enableValue, disableValue);
}

function EnableDisableCartSubmit(submit, enableValue, disableValue) {
    const cartProducts = document.querySelectorAll('.cart-product');

    if (cartProducts.length === 0) {
        submit.disabled = true;
        submit.classList.add('disabled');
        submit.value = disableValue;
    } else {
        submit.disabled = false;
        submit.classList.remove('disabled');
        submit.value = enableValue;
    }
}

// Вызываем функцию при загрузке страницы
document.addEventListener('DOMContentLoaded', updateBuySubmitState);
document.addEventListener('DOMContentLoaded', updateClearCartSubmitState);