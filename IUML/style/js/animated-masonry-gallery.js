$(window).load(function () {
    animated_masonry_gallery_Load();
});

function animated_masonry_gallery_Load()//
{
    var size = 1;
    var button = 1;
    var button_class = "gallery-header-center-right-links-current";
    var normal_size_class = "gallery-content-center-normal";
    var full_size_class = "gallery-content-center-full";
    var $container = $('#gallery-content-center');

    $container.isotope({ itemSelector: 'img' });


    function check_button() {
        $('.gallery-header-center-right-links').removeClass(button_class);
        if (button == 1) {
            $("#filter-all").addClass(button_class);
            $("#gallery-header-center-left-title").html('All');
        }
        if (button == 2) {
            $("#filter-Villas").addClass(button_class);
            $("#gallery-header-center-left-title").html('Villas');
        }
        if (button == 3) {
            $("#filter-Building").addClass(button_class);
            $("#gallery-header-center-left-title").html('Building');
        }
        if (button == 4) {
            $("#filter-Engineering").addClass(button_class);
            $("#gallery-header-center-left-title").html('Engineering');
        }
        if (button == 5) {
            $("#filter-Factory").addClass(button_class);
            $("#gallery-header-center-left-title").html('Factory');
        }
        if (button == 6) {
            $("#filter-Palace").addClass(button_class);
            $("#gallery-header-center-left-title").html('Palace');
        }
    }

    function check_size() {
        $("#gallery-content-center").removeClass(normal_size_class).removeClass(full_size_class);
        if (size == 0) {
            $("#gallery-content-center").addClass(normal_size_class);
            $("#gallery-header-center-left-icon").html('<span class="iconb" data-icon="&#xe23a;"></span>');
        }
        if (size == 1) {
            $("#gallery-content-center").addClass(full_size_class);
            $("#gallery-header-center-left-icon").html('<span class="iconb" data-icon="&#xe23b;"></span>');
        }
        $container.isotope({ itemSelector: 'img' });
    }



    $("#filter-all").click(function () { $container.isotope({ filter: '.all' }); button = 1; check_button(); });
    $("#filter-Villas").click(function () { $container.isotope({ filter: '.Villas' }); button = 2; check_button(); });
    $("#filter-Building").click(function () { $container.isotope({ filter: '.Building' }); button = 3; check_button(); });
    $("#filter-Engineering").click(function () { $container.isotope({ filter: '.Engineering' }); button = 4; check_button(); });
    $("#filter-Factory").click(function () { $container.isotope({ filter: '.Factory' }); button = 5; check_button(); });
    $("#filter-Palace").click(function () { $container.isotope({ filter: '.Palace' }); button = 6; check_button(); });
    
    $("#gallery-header-center-left-icon").click(function () { if (size == 0) { size = 1; } else if (size == 1) { size = 0; } check_size(); });


    check_button();
    check_size();
}