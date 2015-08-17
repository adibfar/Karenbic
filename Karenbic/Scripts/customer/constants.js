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
                sref: "app.design.preorder",
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
                notification_get_fn: 'getUnpayedDesignFactor',
                notification_new_fn: 'newUnpayedDesignFactor',
                notification_minus_fn: 'minusUnpayedDesignFactor',
                submenu: [
                    {
                        text: "صورت حساب خدمات طراحی",
                        sref: "app.design.factor-list",
                        notification_get_fn: 'getUnpayedDesignFactor',
                        notification_new_fn: 'newUnpayedDesignFactor',
                        notification_minus_fn: 'minusUnpayedDesignFactor'
                    },
                    {
                        text: "فاکتور های پرداخت شده",
                        sref: "app.design.payment-list"
                    },
                    {
                        text: "مغایرت های مالی",
                        sref: "app.design.financial-conflict-list"
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
                        text: "یش نمایش فاکتور",
                        sref: "app.design.final-payment-preview"
                    },
                    {
                        text: "فاکتور پرداخت شده",
                        sref: "app.design.checkout-final-payment"
                    },
                    {
                        text: "خطا در پرداخت",
                        sref: "app.design.error-payment"
                    }
                ]
            },
            {
                text: "نمایش سفارشات",
                sref: "app.design.order-list",
                icon: "show-order",
                notification_get_fn: 'getUnreviewedDesign',
                notification_new_fn: 'newUnreviewedDesign',
                notification_minus_fn: 'minusUnreviewedDesign'
            },
            {
                text: "لیست قیمت طراحی",
                sref: "app.design.price-list",
                icon: "price-list"
            },
            {
                text: "پیام ها",
                sref: "#",
                icon: "message",
                notification_get_fn: 'getUnReadMessage',
                notification_new_fn: 'newUnReadMessage',
                notification_minus_fn: 'minusUnReadMessage',
                submenu: [
                    {
                        text: "ارسال پیام جدید",
                        sref: "app.design.send-message-new"
                    },
                    {
                        text: "پیام های من / پاسخ",
                        sref: "app.design.send-message-list",
                        notification_get_fn: 'getUnReadReplyMessage',
                        notification_new_fn: 'newUnReadReplyMessage',
                        notification_minus_fn: 'minusUnReadReplyMessage'
                    },
                    {
                        text: "پیام از مدیر",
                        sref: "app.design.receive-message-list",
                        notification_get_fn: 'getUnReadAdminMessage',
                        notification_new_fn: 'newUnReadAdminMessage',
                        notification_minus_fn: 'minusUnReadAdminMessage'
                    }
                ]
            },
            {
                text: "تنظیمات",
                sref: "#",
                icon: "setting",
                submenu: [
                    {
                        text: "ویرایش اطلاعات کاربری",
                        sref: "app.design.change-profile"
                    },
                    {
                        text: "تغییر رمز عبور",
                        sref: "app.design.change-password"
                    }
                ]
            }
        ],
        menuItems_print: [
            {
                text: "پیش از سفارش",
                sref: "app.print.preorder",
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
                notification_get_fn: 'getUnpayedPrintFactor',
                notification_new_fn: 'newUnpayedPrintFactor',
                notification_minus_fn: 'minusUnpayedPrintFactor',
                submenu: [
                    {
                        text: "صورت حساب خدمات چاپ",
                        sref: "app.print.factor-list",
                        notification_get_fn: 'getUnpayedPrintFactor',
                        notification_new_fn: 'newUnpayedPrintFactor',
                        notification_minus_fn: 'minusUnpayedPrintFactor'
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
                text: "پیگیری سفارش",
                sref: "app.print.order-list",
                icon: "order-track"
            },
            {
                text: "لیست قیمت چاپ",
                sref: "app.print.price-list",
                icon: "price-list"
            },
            {
                text: "پیام ها",
                sref: "#",
                icon: "message",
                notification_get_fn: 'getUnReadMessage',
                notification_new_fn: 'newUnReadMessage',
                notification_minus_fn: 'minusUnReadMessage',
                submenu: [
                    {
                        text: "ارسال پیام جدید",
                        sref: "app.print.send-message-new"
                    },
                    {
                        text: "پیام های من / پاسخ",
                        sref: "app.print.send-message-list",
                        notification_get_fn: 'getUnReadReplyMessage',
                        notification_new_fn: 'newUnReadReplyMessage',
                        notification_minus_fn: 'minusUnReadReplyMessage'
                    },
                    {
                        text: "پیام از مدیر",
                        sref: "app.print.receive-message-list",
                        notification_get_fn: 'getUnReadAdminMessage',
                        notification_new_fn: 'newUnReadAdminMessage',
                        notification_minus_fn: 'minusUnReadAdminMessage'
                    }
                ]
            },
            {
                text: "تنظیمات",
                sref: "#",
                icon: "setting",
                submenu: [
                    {
                        text: "ویرایش اطلاعات کاربری",
                        sref: "app.print.change-profile"
                    },
                    {
                        text: "تغییر رمز عبور",
                        sref: "app.print.change-password"
                    }
                ]
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
            'persian-date': ['/Vendors/persian-date/persian-date-0.1.8.min.js'],
            'jquery-colorbox': ['/Vendors/jquery-colorbox/jquery.colorbox-min.js', '/Vendors/jquery-colorbox/them/colorbox.css'],
            'image-scale': ['/Vendors/image-scale/image-scale.min.js'],
            'cropit': ['/Vendors/cropit/jquery.cropit.js']
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
            },
            {
                name: 'froala',
                files: ['/Vendors/froala-editor/js/froala_editor.min.js',
                    '/Vendors/froala-editor/js/angular-froala.js',
                    '/Vendors/froala-editor/js/froala-sanitize.js',
                    '/Vendors/froala-editor/js/langs/fa.js',
                    '/Vendors/froala-editor/css/froala_editor.min.css',
                    '/Vendors/froala-editor/css/froala_style.min.css',
                    '/Vendors/froala-editor/css/froala_content.min.css',
                    '/Vendors/froala-editor/css/themes/gray.min.css']
            }
        ]
    });