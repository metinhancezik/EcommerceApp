// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showPaymentStatus(status, message) {
    var overlay = document.getElementById('paymentOverlay');
    var statusIcon = document.getElementById('paymentStatusIcon');
    var statusMessage = document.getElementById('paymentStatusMessage');

    statusMessage.textContent = message;

    if (status === 'success') {
        statusIcon.innerHTML = '✅';
        statusIcon.style.color = 'green';
    } else {
        statusIcon.innerHTML = '❌';
        statusIcon.style.color = 'red';
    }

    overlay.style.display = 'flex';

    setTimeout(function () {
        overlay.style.opacity = '0';
        setTimeout(function () {
            overlay.style.display = 'none';
            overlay.style.opacity = '1';
        }, 500);
    }, 5000);
}

// TempData'dan gelen bilgileri kontrol et
document.addEventListener('DOMContentLoaded', function () {
    var paymentStatus = tempData.paymentStatus;
    var paymentMessage = tempData.paymentMessage;

    if (paymentStatus && paymentMessage) {
        showPaymentStatus(paymentStatus, paymentMessage);
    }
});
