/*=========================================================
 * Module: constants.js
 * Define constants to inject across the application
 =========================================================*/
App
    .constant('APP_BASE_URI', '/Customer/')
    .constant('APP_PORTALS', {
        'design': 0,
        'print': 1
    })
    .constant('APP_Portal_Menu', {
        menuItems_design: [
            {
                text: "پیش از سفارش",
                sref: "app.design.dashboard",
                icon: "pre-order"
            },
            {
                text: "ثبت سفارش جدید",
                sref: "app.design.add-order",
                icon: "order"
            },
            {
                text: "صورت حساب",
                sref: "#",
                icon: "billing",
                submenu: [
                    {
                        text: "صورت حساب خدمات طراحی",
                        sref: "app.design.factor-list"
                    },
                    {
                        text: "فاکتور های پرداخت شده",
                        sref: "app.design.payment-list"
                    }
                ],
                activedmenu: [
                     {
                         text: "یش نمایش فاکتور",
                         sref: "app.design.payment-preview"
                     },
                    {
                        text: "فاکتور پرداخت شده",
                        sref: "app.design.checkout-payment"
                    },
                    {
                        text: "خطا در پرداخت",
                        sref: "app.design.error-payment"
                    }
                ]
            },
            {
                text: "پیگیری سفارش",
                sref: "app.design.dashboard",
                icon: "order-track"
            },
            {
                text: "لیست قیمت",
                sref: "app.design.dashboard",
                icon: "price-list"
            },
            {
                text: "پیام ها",
                sref: "app.design.dashboard",
                icon: "message"
            },
            {
                text: "تنظیمات",
                sref: "app.design.dashboard",
                icon: "setting"
            }
        ],
        menuItems_print: [
            {
                text: "پیش از سفارش",
                sref: "app.print.dashboard1",
                icon: "pre-order"
            },
            {
                text: "ثبت سفارش جدید",
                sref: "app.print.add-order",
                icon: "order"
            },
            {
                text: "صورت حساب",
                sref: "#",
                icon: "billing",
                submenu: [
                    {
                        text: "صورت حساب خدمات چاپ",
                        sref: "app.print.factor-list"
                    },
                    {
                        text: "فاکتور های پرداخت شده",
                        sref: "app.print.payment-list"
                    }
                ],
                activedmenu: [
                    {
                        text: "یش نمایش فاکتور",
                        sref: "app.print.payment-preview"
                    },
                    {
                        text: "فاکتور پرداخت شده",
                        sref: "app.print.checkout-payment"
                    },
                    {
                        text: "خطا در پرداخت",
                        sref: "app.print.error-payment"
                    }
                ]
            },
            {
                text: "نمایش سفارشات",
                sref: "app.print.order-list",
                icon: "show-order"
            },
            {
                text: "لیست قیمت",
                sref: "app.print.dashboard3",
                icon: "price-list"
            },
            {
                text: "پیام ها",
                sref: "app.print.dashboard3",
                icon: "message"
            },
            {
                text: "تنظیمات",
                sref: "app.print.dashboard3",
                icon: "setting"
            }
        ]
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
            'modernizr': ['/Vendors/modernizr/modernizr.js'],
            'icons': ['/Vendors/fontawesome/css/font-awesome.min.css', '/Vendors/simplelineicons/simple-line-icons.css'],
            'fastclick': ['/Vendors/fastclick/fastclick.js'],
            'animo': ['/Vendors/animo/animo.min.js', '/Vendors/animo/animateanimo.css'],
            'whirl': ['/Vendors/whirl/whirl.css'],
            'chosen': ['/Vendors/chosen/chosen.jquery.min.js', '/Vendors/chosen/chosen.css'],
            'ddlist': ['/Vendors/ddlist/ddlist.min.js', '/Vendors/ddlist/ddlist.css'],
            'stepper': ['/Vendors/Numeric-Stepper/jquery.stepper.min.js', '/Vendors/Numeric-Stepper/jquery.stepper.min.css'],
            'nicefileinput': ['/Vendors/nicefileinput/jquery.nicefileinput.min.js', '/Vendors/nicefileinput/nicefileinput.css'],
            'colorpicker': ['/Vendors/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js', '/Vendors/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css'],
            'classyloader': ['/Vendors/classyloader/js/jquery.classyloader.min.js'],
            'jquery-ui': ['/Vendors/jqueryui/js/jquery-ui-1.10.4.custom.min.js', '/Vendors/jqueryui/css/smoothness/jquery-ui-1.10.4.custom.min.css'],
            'inputmask': ['/Vendors/inputmask/jquery.inputmask.bundle.min.js'],
            'mousewheel': ['/Vendors/scrollbar/js/minified/jquery.mousewheel-3.0.6.min.js'],
            'scrollbar': ['/Vendors/scrollbar/css/jquery.mCustomScrollbar.css', '/Vendors/scrollbar/js/minified/jquery.mCustomScrollbar.min.js'],
            'jquery-resize': ['/Vendors/jquery-resize/jquery.ba-resize.min.js'],
            'jquery-ui-datepicker': ['/Vendors/jquery.ui.datepicker/jquery.ui.datepicker-cc.all.min.js'],
            'tooltipster': ['/Vendors/tooltipster/js/jquery.tooltipster.min.js', '/Vendors/tooltipster/css/tooltipster.css', '/Vendors/tooltipster/css/themes/tooltipster-light.css'],
            'persian-date': ['/Vendors/persian-date/persian-date-0.1.8.min.js']
        },
        modules: [
            {
                name: 'toaster',
                files: ['/Vendors/toaster/toaster.js',
                  '/Vendors/toaster/toaster.css']
            },
            {
                name: 'gridster',
                files: ['/Vendors/angular-gridster/angular-gridster.min.js',
                    '/Vendors/angular-gridster/angular-gridster.min.css']
            },
            {
                name: 'ngDialog',
                files: ['/Vendors/ngDialog/js/ngDialog.min.js',
                    '/Vendors/ngDialog/css/ngDialog.min.css',
                    '/Vendors/ngDialog/css/ngDialog-theme-default.min.css']
            }
        ]
    });