/*=========================================================
 * Module: constants.js
 * Define constants to inject across the application
 =========================================================*/
App
    .constant('APP_BASE_URI', '/Admin/')
    .constant('APP_PORTALS', {
        'design': 0,
        'print': 1
    })
    .constant('APP_Portal_Menu', {
        menuItems_design: [
            {
                text: "سفارشات",
                sref: "#",
                icon: "show-order",
                submenu: [
                  {
                      text: "سفارشات جدید",
                      sref: "app.design.new-order-list"
                  },
                  {
                      text: "سفارشات در دست اقدام",
                      sref: "app.design.ongoing-order-list"
                  },
                  {
                      text: "سفارشات انجام شده",
                      sref: "app.design.finished-order-list"
                  },
                  {
                      text: "سفارشات لغو شده",
                      sref: "app.design.canceled-order-list"
                  }
                ],
                activedmenu: [
                    {
                        text: "ارسال فایل های طراحی شده",
                        sref: "app.design.send-order-design"
                    },
                    {
                        text: "نمایش فایل های طراحی شده",
                        sref: "app.design.show-order-design"
                    }
                ]
            },
            {
                text: "تراکنش مالی",
                sref: "#",
                icon: "financial-transaction",
                submenu: [
                    {
                        text: "لیست پرداخت های مشتریان",
                        sref: "app.design.payment-list"
                    },
                    {
                        text: "صورت حساب مشتریان",
                        sref: "app.design.factor-list"
                    }
                ]
            },
            {
                text: "مدیریت فرم ها",
                sref: "#",
                icon: "form",
                submenu: [
                  {
                      text: "گروه بندی فرم ها",
                      sref: "app.design.form-groups"
                  },
                  {
                      text: "ثبت فرم جدید",
                      sref: "app.design.add-form"
                  },
                  {
                      text: "فرم های ثبت شده",
                      sref: "app.design.forms-list"
                  }
                ],
                activedmenu: [
                    {
                        text: "ویرایش فرم",
                        sref: "app.design.edit-form"
                    }
                ]
            },
            {
                text: "مشتریان",
                sref: "#",
                icon: "customers",
                submenu: [
                    {
                        text: "گروه مشتریان",
                        sref: "app.design.customer-group"
                    },
                    {
                        text: "لیست مشتریان",
                        sref: "app.design.customer"
                    }
                ]
            },
            {
                text: "لیست قیمت",
                sref: "app.design.price-list",
                icon: "price-list"
            },
            {
                text: "نمونه کارها",
                sref: "#",
                icon: "price-list",
                submenu: [
                    {
                        text: "گروه بندی",
                        sref: "app.design.portfolio-categories"
                    },
                    {
                        text: "ثبت نمونه کار جدید",
                        sref: "app.design.portfolio-add"
                    },
                    {
                        text: "نمونه کارهای ثبت شده",
                        sref: "app.design.portfolios"
                    }
                ]
            },
            {
                text: "پیام ها",
                sref: "#",
                icon: "message",
                submenu: [
                    {
                        text: "ارسال پیام جدید",
                        sref: "app.design.send-message-new"
                    },
                    {
                        text: "پیام های ارسالی",
                        sref: "app.design.send-message-list"
                    },
                    {
                        text: "پیام های دریافتی",
                        sref: "app.design.receive-message-list"
                    }
                ]
            },
            {
                text: "تنظیمات",
                sref: "#",
                icon: "setting",
                submenu: [
                    {
                        text: "متن پیش از سفارش",
                        sref: "app.design.preorder"
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
                text: "سفارشات",
                sref: "#",
                icon: "show-order",
                submenu: [
                  {
                      text: "سفارشات جدید",
                      sref: "app.print.new-order-list"
                  },
                  {
                      text: "ثبت وضعیت سفارشات",
                      sref: "app.print.ongoing-order-list"
                  },
                  {
                      text: "سفارشات انجام شده",
                      sref: "app.print.finished-order-list"
                  },
                  {
                      text: "سفارشات لغو شده",
                      sref: "app.print.canceled-order-list"
                  }
                ]
            },
            {
                text: "تراکنش مالی",
                sref: "#",
                icon: "financial-transaction",
                submenu: [
                  {
                      text: "لیست پرداخت های مشتریان",
                      sref: "app.print.payment-list"
                  },
                  {
                      text: "صورت حساب مشتریان",
                      sref: "app.print.factor-list"
                  }
                ]
            },
            {
                text: "مدیریت فرم ها",
                sref: "#",
                icon: "form",
                submenu: [
                    {
                        text: "گروه بندی فرم ها",
                        sref: "app.print.form-groups"
                    },
                    {
                        text: "ثبت فرم جدید",
                        sref: "app.print.add-form"
                    },
                    {
                        text: "فرم های ثبت شده",
                        sref: "app.print.forms-list"
                    }
                ],
                activedmenu: [
                    {
                        text: "ویرایش فرم",
                        sref: "app.print.edit-form"
                    }
                ]
            },
            {
                text: "مشتریان",
                sref: "#",
                icon: "customers",
                submenu: [
                    {
                         text: "گروه مشتریان",
                         sref: "app.print.customer-group"
                    },
                    {
                        text: "لیست مشتریان",
                        sref: "app.print.customer"
                    }
                ]
            },
            {
                text: "لیست قیمت",
                sref: "app.print.price-list",
                icon: "price-list"
            },
            {
                text: "نمونه کارها",
                sref: "#",
                icon: "price-list",
                submenu: [
                    {
                        text: "گروه بندی",
                        sref: "app.print.portfolio-categories"
                    },
                    {
                        text: "ثبت نمونه کار جدید",
                        sref: "app.print.portfolio-add"
                    },
                    {
                        text: "نمونه کارهای ثبت شده",
                        sref: "app.print.portfolios"
                    }
                ]
            },
            {
                text: "پیام ها",
                sref: "#",
                icon: "message",
                submenu: [
                    {
                        text: "ارسال پیام جدید",
                        sref: "app.print.send-message-new"
                    },
                    {
                        text: "پیام های ارسالی",
                        sref: "app.print.send-message-list"
                    },
                    {
                        text: "پیام های دریافتی",
                        sref: "app.print.receive-message-list"
                    }
                ]
            },
            {
                text: "تنظیمات",
                sref: "#",
                icon: "setting",
                submenu: [
                    {
                        text: "متن پیش از سفارش",
                        sref: "app.print.preorder"
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
            'image-scale': ['/Vendors/image-scale/image-scale.min.js']
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
                files: ['/Vendors/froala/js',
                    '/Vendors/froala-editor/css/froala_editor.min.css',
                    '/Vendors/froala-editor/css/froala_style.min.css',
                    '/Vendors/froala-editor/css/froala_content.min.css',
                    '/Vendors/froala-editor/css/themes/gray.min.css']
            }
        ]
    });