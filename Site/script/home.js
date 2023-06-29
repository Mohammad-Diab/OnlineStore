$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        if (!islogin) {
            window.location.href += '../welcome'
        } else {
            requestToServer('Store/GetTodayDeals', 'get', '', (data) => {
                requestToServer('Store/GetTopProducts', 'get', '', (data2) => {
                    if (data2) {
                        fillGrid(data2, 'topSeller');
                    }
                });
                if (data) {
                    fillGrid(data, 'todayDeals');
                }
            });

        }
    })
});

function fillGrid(data, gridId) {
    let grid = $(`#${gridId}`);
    grid.empty();
    let htmlVal = '';
    if (data?.length) {
        for (let i = 0; i < data.length; i++) {
            const el = data[i];
            htmlVal += getObject(el);
        }
    } else {
        htmlVal += '<p style="flex: 1" class="text-center fs-4">';
        htmlVal += 'لا يوجد عناصر';
        htmlVal += '</p>';
    }
    grid.html(htmlVal);
    if (data?.length) {
        for (let i = 0; i < data.length; i++) {
            GetImage(data[i].image, `image-${data[i].id}`, i);
        }
    }

}

function getObject(el) {
    let htmlVal = `<a href="../product?id=${el.id}" class="card card-width">`
    htmlVal += `<div class="image-container">`
    htmlVal += `<img id="image-${el.id}" class="card-img-top" alt="Item 2">`
    htmlVal += `</div>`
    htmlVal += `<div class="card-footer d-flex justify-content-between align-items-center">`
    htmlVal += `<h6 class="card-title mb-0">${el.name}</h6>`
    htmlVal += `<p dir="ltr" class="card-text">${el.price} SYP</p>`
    htmlVal += `</div>`
    htmlVal += '</a>';

    return htmlVal
}

function GetImage(path, id, index) {
    requestToServer('Store/GetImage?path=' + path, 'get', '', (data) => {
        $(`#${id}`).attr('src', data);
    })
}

