﻿/*=========================================================
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
                      sref: "app.dashboard"
                  },
                  {
                      text: "سفارشات در دست اقدام",
                      sref: "app.design.dashboard"
                  },
                  {
                      text: "سفارشات انجام شده",
                      sref: "app.design.dashboard"
                  }
                ]
            },
            {
                text: "تراکنش مالی",
                sref: "#",
                icon: "financial-transaction",
                submenu: [
                  {
                      text: "صورت حساب مشتریان",
                      sref: "app.design.dashboard"
                  },
                  {
                      text: "ثبت مغایرت مالی",
                      sref: "app.design.dashboard"
                  }
                ]
            },
            {
                text: "مدیریت فرم ها",
                sref: "#",
                icon: "form",
                submenu: [
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
                sref: "app.design.dashboard",
                icon: "customers"
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
            text: "مدیریت فرم ها",
            sref: "#",
            icon: "form",
            submenu: [
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
        }]
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
            'jquery-ui': ['/Vendors/jqueryui/js/jquery-ui-1.10.4.custom.min.js', '/Vendors/jqueryui/css/ui-lightness/jquery-ui-1.10.4.custom.min.css', , '/Vendors/touch-punch/jquery.ui.touch-punch.min.js'],
            'inputmask': ['/Vendors/inputmask/jquery.inputmask.bundle.min.js'],
            'mousewheel': ['/Vendors/scrollbar/js/minified/jquery.mousewheel-3.0.6.min.js'],
            'scrollbar': ['/Vendors/scrollbar/css/jquery.mCustomScrollbar.css', '/Vendors/scrollbar/js/minified/jquery.mCustomScrollbar.min.js'],
            'jquery-resize': ['/Vendors/jquery-resize/jquery.ba-resize.min.js']
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