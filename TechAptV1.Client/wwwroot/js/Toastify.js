
window.showToast = (options) => {
    Toastify({
        text: options.text,
        duration: options.duration,
        close: options.close,
        gravity: options.gravity,
        position: options.position,
        style: {
            background: options.backgroundColor
        }
    }).showToast();
};
