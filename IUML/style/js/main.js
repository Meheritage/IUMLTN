$(document).ready(function () {
    gallery_Load();
});

function gallery_Load()//
{
    new Photostack(document.getElementById('photostack-3'), {
        callback: function (item) {
            //console.log(item)
        }
    });
}