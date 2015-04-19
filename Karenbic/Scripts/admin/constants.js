/*=========================================================
 * Module: constants.js
 * Define constants to inject across the application
 =========================================================*/
App
    .constant('APP_PORTALS', {
        'design': 0,
        'print': 1
    })
    .constant('APP_COLORS', {
        'primary': '#5d9cec',
        'success': '#27c24c',
        'info': '#23b7e5',
        'warning': '#ff902b',
        'danger': '#f05050',
        'inverse': '#131e26',
        'green': '#37bc9b',
        'pink': '#f532e5',
        'purple': '#7266ba',
        'dark': '#3a3f51',
        'yellow': '#fad732',
        'gray-darker': '#232735',
        'gray-dark': '#3a3f51',
        'gray': '#dde6e9',
        'gray-light': '#e4eaec',
        'gray-lighter': '#edf1f2'
    })
    .constant('APP_MEDIAQUERY', {
        'desktopLG': 1200,
        'desktop': 992,
        'tablet': 768,
        'mobile': 480,
        'iphone': 320
    })
    .constant('APP_REQUIRES', {
        scripts: {
            'jquery': ['/Vendors/jquery-1.11.2/jquery-1.11.2.min.js', 'jquery-migrate-1.2.1.min.js'],
            'icons': ['/Vendors/fontawesome/css/font-awesome.min.css', '/Vendors/simplelineicons/simple-line-icons.css'],
            'modernizr': ['/Vendors/modernizr/modernizr.js'],
            'fastclick': ['/Vendors/fastclick/fastclick.js'],
            'chosen': ['/Vendors/chosen/chosen.jquery.min.js', '/Vendors/chosen/chosen.min.css'],
            'filestyle': ['/Vendors/filestyle/bootstrap-filestyle.min.js'],
            'csspiner': ['/Vendors/csspinner/csspinner.min.css'],
            'animo': ['/Vendors/animo/animo.min.js'],
            'classyloader': ['/Vendors/classyloader/js/jquery.classyloader.min.js'],
            'jquery-ui': ['/Vendors/jqueryui/js/jquery-ui-1.10.4.custom.min.js', '/Vendors/jqueryui/css/ui-lightness/jquery-ui-1.10.4.custom.min.css', , '/Vendors/touch-punch/jquery.ui.touch-punch.min.js'],
            'inputmask': ['/Vendors/inputmask/jquery.inputmask.bundle.min.js'],
            'datatables': ['/Vendors/datatable/media/js/jquery.dataTables.min.js', '/Vendors/datatable/extensions/datatable-bootstrap/css/dataTables.bootstrap.css'],
            'mousewheel': ['/Vendors/scrollbar/js/minified/jquery.mousewheel-3.0.6.min.js'],
            'scrollbar': ['/Vendors/scrollbar/css/jquery.mCustomScrollbar.css', '/Vendors/scrollbar/js/minified/jquery.mCustomScrollbar.min.js']
        },
        modules: [
            { name: 'toaster', files: ['/Vendors/toaster/toaster.js', '/Vendors/toaster/toaster.css'] }
        ]
    });