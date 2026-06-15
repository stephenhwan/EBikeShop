// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const myModal = new bootstrap.Modal('#eBikeModal', {
    keyboard: false
})

/**
 * Hàm mở modal nha mấy ní
 * @param {any} title Tiêu đề của Modal
 * @param {any} body Nội dung HTML của Modal
 * @param {any} size Kích thước của Modal
 */
function openModal(title, body, size) {
    let eleTitle = document.getElementById("hModalTitle");
    if (eleTitle) {
        eleTitle.innerText = title;
    }
    let divModalBody = document.getElementById("divModalBody");
    if (divModalBody) {
        divModalBody.innerHTML = body;
    }
    myModal.show();
}

