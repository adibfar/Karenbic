﻿/**=========================================================
 * Module: order-add.js
 * Send New Order
 =========================================================*/

App.controller('AddOrderController', ['$scope', '$http', 'APP_BASE_URI', '$upload', 'toaster', 'ngDialog', '$state', '$modal',
    function ($scope, $http, baseUri, $upload, toaster, ngDialog, $state, $modal) {

        /*=-=-=-=-= Start Fetch Forms =-=-=-=-=*/
        $scope.formGroups = [];
        $scope.formGroups_column1 = [];
        $scope.formGroups_column2 = [];
        $scope.formGroups_column3 = [];

        $scope.fetchFormGroups = function () {
            $scope.fetchFormsLoading = true;

            $http.get(baseUri + 'FormGroup/Get', {
                params: {
                    portal: $scope.isDesignPortal() ? 1 : 2
                }
            })
            .success(function (data, status, headers, config) {
                $scope.formGroups = data.Data;

                //Columns#1
                $scope.formGroups_column1 = _.filter($scope.formGroups, function (item) {
                    return item.Column == 1;
                });

                //Columns#2
                $scope.formGroups_column2 = _.filter($scope.formGroups, function (item) {
                    return item.Column == 2;
                });

                //Columns#3
                $scope.formGroups_column3 = _.filter($scope.formGroups, function (item) {
                    return item.Column == 3;
                });

                $scope.fetchFormsLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchFormsLoading = false;
            });
        };

        $scope.fetchFormGroups();
        /*=-=-=-=-= End Fetch Forms =-=-=-=-=*/

        /*=-=-=-=-= Start Select Form =-=-=-=-=*/
        $scope.selectedForm = null;

        $scope.selectForm = function ($event, form) {
            $scope.selectedForm = form;
            $('.formGroups-list > li').fadeOut(300);
            window.setTimeout(function () {
                $scope.fetchFields();
            }, 300);
        };

        $scope.unSelectForm = function ($event, form) {
            $scope.selectedForm = null;
            $scope.form = {};
            $scope.fromSumbited = false;
            $scope.specialCreativity = false;
            $('.formGroups-list > li').fadeIn(300);
        };
        /*=-=-=-=-= Start Select Form =-=-=-=-=*/

        /*=-=-=-=-= Start Fetch Fields =-=-=-=-=*/
        $scope.form = {};
        $scope.fetchFields = function () {
            $scope.fetchFieldsLoading = true;
            $http.get(baseUri + 'Form/GetFields', {
                params: {
                    id: $scope.selectedForm.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.form = data.Data;
                $scope.fetchFieldsLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchFieldsLoading = false;
            });
        };
        /*=-=-=-=-= End Fetch Fields =-=-=-=-=*/

        /*=-=-=-=-= Start Select Special Desgin =-=-=-=-=*/
        $scope.specialCreativity = false;
        $scope.selectSpecialCreativity = function () {
            if ($scope.specialCreativity == false) {
                ngDialog.open({
                    template: '<div style="font-family:yekan;font-size:14px;">' +
                        '<p>' +
                        'در صورت انتخاب این گزینه، هزینه طراحی با توجه به تعرفه خلاقیت ویژه محاسبه می گردد.' +
                        '</p>' +
                        '<p>' +
                        'برای مشاهده تعرفه خلاقیت ویژه به لیست قیمت مراجعه نمایید.' +
                        '</p>' +
                        '<div style="text-align:left;">' +
                        '<button class="btn btn-warning" ng-click="close()" type="button">خیر</button>' +
                        '<button class="btn btn-primary" ng-click="confirm()" type="button" style="margin-right:4px;">بلی</button>' +
                        '</div></div>',
                    plain: true,
                    showClose: false,
                    controller: ['$scope', 'ngDialog', function ($scope, ngDialog) {
                        $scope.close = function () {
                            $scope.closeThisDialog(0);
                        };
                        $scope.confirm = function () {
                            $scope.closeThisDialog(1);
                        };
                    }],
                    preCloseCallback: function (value) {
                        if (value != 1) return true;
                        $scope.specialCreativity = true;
                        return true;
                    }
                });
            }
            else {
                $scope.specialCreativity = false;
            }
        };
        /*=-=-=-=-= End Select Special Desgin =-=-=-=-=*/

        /*=-=-=-=-= Start On File Select =-=-=-=-=*/
        $scope.uploadingFile = 0;
        $scope.onFileSelect = function (item, $files) {
            var $file = $files[0];
            var extension = $file.name.substr($file.name.lastIndexOf('.') + 1).toLowerCase();

            if (_.find(item.data.fileTypes, function (type) {
                return type.Extention.toLowerCase() == extension;
            }) == undefined) {
                error_msg = "فرمت فایل می تواند";
                for (i = 0; i < item.data.fileTypes.length; i++) {
                    error_msg += item.data.fileTypes[i].Extention.toUpperCase();
                    if (i != item.data.fileTypes.length - 1) {
                        error_msg += " یا ";
                    }
                }
                error_msg += " باشد.";
                toaster.pop('error', error_msg);
                return;
            }
            
            if (item.data.sizeLimits == true) {
                if ($file.size < item.data.minSize * 1024) {
                    error_msg = "حداقل حجم فایل ";
                    error_msg += item.data.minSize;
                    error_msg += "کیلو بایت می باشد ";
                    toaster.pop('error', error_msg);
                    return;
                }
                else if ($file.size > item.data.maxSize * 1024) {
                    error_msg = "حداکثر حجم فایل ";
                    error_msg += item.data.maxSize;
                    error_msg += "کیلو بایت می باشد ";
                    toaster.pop('error', error_msg);
                    return;
                }
            }

            $scope.uploadingFile++;
            item.uploading = true;
            $upload.upload({
                url: baseUri + 'Order/Add_UploadFile',
                file: $file,
                data: {
                    fieldId: item.data.id
                }
            }).success(function (data, status, headers, config) {
                item.value = data;
                $scope.uploadingFile--;
                item.uploading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.uploadingFile--;
                item.uploading = false;
            });
        };
        /*=-=-=-=-= End On File Select =-=-=-=-=*/

        /*=-=-=-=-= Start Extended File Uploader =-=-=-=-=*/
        $scope.selectedExtendedFileUploader_item = null;
        $scope.selectedExtendedFileUploader_element = null;
        $scope.extendedFileUploaderPopup = null;

        $scope.onExtendedFileUploaderClick = function ($event, item) {
            $scope.selectedExtendedFileUploader_item = item;
            $scope.selectedExtendedFileUploader_element = $event.currentTarget;
            $scope.extendedFileUploaderPopup = $('#extended-file-uploader-popup').bPopup();
        };

        $scope.extendedFileUploader_selectFormLocalFile = function () {
            $scope.extendedFileUploaderPopup.close();
            $($scope.selectedExtendedFileUploader_element).closest('.extended-file-uploader').find("input[type=file]").click();
        };

        $scope.extendedFileUploader_selectFormLocalFile_onFileSelect = function (item, $files) {
            var $file = $files[0];
            var extension = $file.name.substr($file.name.lastIndexOf('.') + 1).toLowerCase();

            if (_.find(item.data.fileTypes, function (type) {
                return type.Extention.toLowerCase() == extension;
            }) == undefined) {
                error_msg = "فرمت فایل می تواند";
                for (i = 0; i < item.data.fileTypes.length; i++) {
                    error_msg += item.data.fileTypes[i].Extention.toUpperCase();
                    if (i != item.data.fileTypes.length - 1) {
                        error_msg += " یا ";
                    }
                }
                error_msg += " باشد.";
                toaster.pop('error', error_msg);
                return;
            }

            if (item.data.sizeLimits == true) {
                if ($file.size < item.data.minSize * 1024) {
                    error_msg = "حداقل حجم فایل ";
                    error_msg += item.data.minSize;
                    error_msg += "کیلو بایت می باشد ";
                    toaster.pop('error', error_msg);
                    return;
                }
                else if ($file.size > item.data.maxSize * 1024) {
                    error_msg = "حداکثر حجم فایل ";
                    error_msg += item.data.maxSize;
                    error_msg += "کیلو بایت می باشد ";
                    toaster.pop('error', error_msg);
                    return;
                }
            }

            $scope.uploadingFile++;
            item.uploading = true;
            $upload.upload({
                url: baseUri + 'Order/Add_UploadFile2',
                file: $file,
                data: {
                    fieldId: item.data.id
                }
            }).success(function (data, status, headers, config) {
                item.value = data;
                item.valueType = 1;
                item.valueText = $file.name;
                $scope.uploadingFile--;
                item.uploading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.uploadingFile--;
                item.uploading = false;
            });
        };

        $scope.extendedFileUploader_selectFromDesign = function () {
            $scope.extendedFileUploaderPopup.close();
            
            var modalInstance = $modal.open({
                templateUrl: '/SelectDesignContent.html',
                controller: extendedFileUploader_selectFromDesign_ModalCtrl,
                size: 'lg'
            });

            modalInstance.result.then(function (result) {
                $scope.selectedExtendedFileUploader_item.value = result.Id;
                $scope.selectedExtendedFileUploader_item.valueType = 2;
                $scope.selectedExtendedFileUploader_item.valueText = result.Form.Title;
            }, function () {
            });
        };

        var extendedFileUploader_selectFromDesign_ModalCtrl = ['$scope', '$http', '$modalInstance', function ($scope, $http, $modalInstance) {

            $scope.searchFields = {
                orderId: '',
                startDate: '',
                endDate: ''
            };
            $scope.orders = [];
            $scope.pages = [];
            $scope.pageCount = 0;
            $scope.pageIndex = 1;

            $scope.fetchOrders = function (pageIndex) {
                $scope.fetchLoading = true;

                $http.get(baseUri + 'Order/Add_GetDesignOrder', {
                    params: {
                        orderId: $scope.searchFields.orderId,
                        startDate: $scope.searchFields.startDate,
                        endDate: $scope.searchFields.endDate,
                        pageIndex: pageIndex
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.orders = data.Data.List;
                    $scope.pageCount = data.Data.PageCount;
                    $scope.pageIndex = data.Data.PageIndex;
                    $scope.resultCount = data.Data.ResultCount;
                    $scope.generatePagation();
                    $scope.fetchLoading = false;
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                    }
                    $scope.fetchLoading = false;
                });
            };

            $scope.search = function () {
                $scope.pageIndex = 1;
                $scope.fetchOrders(1);
            }

            $scope.generatePagation = function () {
                $scope.pages = [];
                if ($scope.pageIndex - 2 > 0) $scope.pages.push($scope.pageIndex - 2);
                if ($scope.pageIndex - 1 > 0) $scope.pages.push($scope.pageIndex - 1);
                $scope.pages.push($scope.pageIndex);
                if ($scope.pageIndex + 1 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 1);
                if ($scope.pageIndex + 2 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 2);
            };

            $scope.changePage = function (index) {
                $scope.fetchOrders($scope.pages[index]);
            };

            $scope.nextPage = function () {
                if ($scope.pageIndex < $scope.pageCount && $scope.fetchLoading == false) {
                    $scope.fetchOrders($scope.pageIndex + 1);
                }
            }

            $scope.prevPage = function () {
                if ($scope.pageIndex > 1 && $scope.fetchLoading == false) {
                    $scope.fetchOrders($scope.pageIndex - 1);
                }
            };

            $scope.select = function (index) {
                $modalInstance.close($scope.orders[index]);
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };

        }];
        /*=-=-=-=-= End Extended File Uploader =-=-=-=-=*/

        /*=-=-=-=-= Start Select Transport Type =-=-=-=-=*/
        $scope.transportType = {
            none: 0,
            bus: 1,
            tipax: 2,
            bikeCycle: 3,
            physical: 4
        };

        $scope.selectedTransport = $scope.transportType.none;

        $scope.selectTransport = function (value) {
            $scope.selectedTransport = value;
        };
        /*=-=-=-=-= End Select Transport Type =-=-=-=-=*/

        /*=-=-=-=-= Start Validate Field =-=-=-=-=*/

        $scope.validateForm = function () {
            var valide = true;

            _.each($scope.form.fields, function (item) {
                switch (item.type) {
                    case 0:
                        if ($scope.validateForm_TextBox(item) == false) {
                            valide = false;
                        }
                        break;

                    case 1:
                        if ($scope.validateForm_TextArea(item) == false) {
                            valide = false;
                        }
                        break;

                    case 2:
                        if ($scope.validateForm_Nummeric(item) == false) {
                            valide = false;
                        }
                        break;

                    case 3:
                        if ($scope.validateForm_ColorPicker(item) == false) {
                            valide = false;
                        }
                        break;

                    case 4:
                        if ($scope.validateForm_FileUploader(item) == false) {
                            valide = false;
                        }
                        break;

                    case 5:
                        break;

                    case 6:
                        if ($scope.validateForm_WebUrl(item) == false) {
                            valide = false;
                        }
                        break;

                    case 7:
                        if ($scope.validateForm_DatePicker(item) == false) {
                            valide = false;
                        }
                        break;

                    case 8:
                        if ($scope.validateForm_DropDown(item) == false) {
                            valide = false;
                        }
                        break;

                    case 9:
                        if ($scope.validateForm_MultipleChoice(item) == false) {
                            valide = false;
                        }
                        break;

                    case 10:
                        break;

                    case 11:
                        if ($scope.validateForm_ExtendedFileUploader(item) == false) {
                            valide = false;
                        }
                        break;
                }
            });

            if ($scope.isPrintPortal() == true && $scope.selectedTransport == $scope.transportType.none) {
                valide = false;
            }

            return valide;
        };

        $scope.validateForm_TextBox = function (item) {
            var valide = true;
            if (item.data.isRequired == true &&
                (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')) {
                valide = false;
            }
            if (item.data.characterLimits == true) {
                if (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '') valide = false;
                else {
                    if (item.value.length < item.data.minCharacters) valide = false;
                    if (item.value.length > item.data.maxCharacters) valide = false;
                }
            }
            return valide;
        };

        $scope.validateForm_TextArea = function (item) {
            var valide = true;
            if (item.data.isRequired == true &&
                (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')) {
                valide = false;
            }
            if (item.data.characterLimits == true) {
                if (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '') valide = false;
                else {
                    if (item.value.length < item.data.minCharacters) valide = false;
                    if (item.value.length > item.data.maxCharacters) valide = false;
                }
            }
            return valide;
        };

        $scope.validateForm_Nummeric = function (item) {
            var valide = true;
            if (item.data.isRequired == true && isNaN(item.value) == true) {
                valide = false;
            }
            if (item.data.limits == true) {
                if (isNaN(item.value) == true) valide = false;
                else {
                    if (item.value < item.data.min) valide = false;
                    if (item.value > item.data.max) valide = false;
                }
            }
            return valide;
        };

        $scope.validateForm_ColorPicker = function (item) {
            var valide = true;
            var re = /^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$/;
            if (item.data.isRequired == true) {
                if (item.value == '' || re.test(item.value) == false)
                    valide = false;
            }
            else {
                if (item.value != undefined && item.value != '' && re.test(item.value) == false)
                    valide = false;
            }
            return valide;
        };

        $scope.validateForm_FileUploader = function (item) {
            var valide = true;
            if (item.data.isRequired == true &&
                (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')) {
                valide = false;
            }
            return valide;
        };

        $scope.validateForm_WebUrl = function (item) {
            var valide = true;
            var re = /^((http|https):\/\/)?[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:\/~+#-]*[\w@?^=%&amp;\/~+#-])?$/;
            if (item.data.isRequired == true) {
                if (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '') valide = false;
                else if (re.test(item.value) == false) valide = false;
            }
            else {
                if (item.value != null && item.value != undefined && item.value != '' &&
                    item.value.trim() != '' && re.test(item.value) == false)
                    valide = false;
            }
            return valide;
        };

        $scope.validateForm_DatePicker = function (item) {
            var valide = true;

            if (item.data.isRequired == true &&
                (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')) {
                valide = false;
            }

            if (item.data.limits == true &&
                item.value != null &&
                item.value != undefined &&
                item.value != '' &&
                item.value.trim() != '') {
                var segments = item.value.split('/');
                var min = persianDate().subtract('days', item.data.min).hour(0).minute(0).seconds(0);
                var max = persianDate().add('days', item.data.max).hour(23).minute(59).seconds(59);
                var day = persianDate([Number(segments[0]), Number(segments[1]), Number(segments[2])]);

                if (min.toDate() > day.toDate() || max.toDate() < day.toDate()) {
                    valide = false;
                }
            }

            return valide;
        };

        $scope.validateForm_DropDown = function (item) {
            var valide = true;
            if (item.data.isRequired == true && item.value == null) {
                valide = false;
            }
            return valide;
        };

        $scope.validateForm_MultipleChoice = function (item) {
            var valide = true;
            if (item.data.isRequired == true && item.value == null) {
                valide = false;
            }
            return valide;
        };

        $scope.validateForm_ExtendedFileUploader = function (item) {
            var valide = true;
            if (item.data.isRequired == true &&
                (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')) {
                valide = false;
            }
            return valide;
        };

        /*=-=-=-=-= End Validate Field =-=-=-=-=*/

        /*=-=-=-=-= Start Form Error =-=-=-=-=*/
        $scope.formErrors = [];

        $scope.getFormErrors = function () {
            $scope.formErrors = [];

            _.each($scope.form.fields, function (item) {
                switch (item.type) {
                    case 0:
                        $scope.textBox_error(item);
                        break;

                    case 1:
                        $scope.textArea_error(item);
                        break;

                    case 2:
                        $scope.numeric_error(item);
                        break;

                    case 3:
                        $scope.colorPicker_error(item);
                        break;

                    case 4:
                        $scope.fileUploader_error(item);
                        break;

                    case 5:
                        break;

                    case 6:
                        $scope.webUrl_error(item);
                        break;

                    case 7:
                        $scope.datePicker_error(item);
                        break;

                    case 8:
                        $scope.dropDown_error(item);
                        break;

                    case 9:
                        $scope.multipleChoice_error(item);
                        break;

                    case 10:
                        break;

                    case 11:
                        $scope.extendedFileUploader_error(item);
                        break;
                }
            });

            if ($scope.isPrintPortal() == true && $scope.selectedTransport == $scope.transportType.none) {
                $scope.formErrors.unshift("روش دریافت سفارش انتخاب نشده است.");
            }

            return $scope.formErrors;
        };

        $scope.textBox_error = function (item) {
            if (item.data.isRequired == true &&
                (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')) {
                $scope.formErrors.unshift("فیلد " + item.data.title + " اجباری می باشد");
            }
            if (item.data.characterLimits == true) {
                if (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '') valide = false;
                else {
                    if (item.value.length < item.data.minCharacters)
                        $scope.formErrors.unshift("حداقل تعداد کاراکتر فیلد " + item.data.title + " " + item.data.minCharacters + "حرف می باشد");
                    if (item.value.length > item.data.maxCharacters)
                        $scope.formErrors.unshift("حداکثر تعداد کاراکتر فیلد " + item.data.title + " " + item.data.maxCharacters + "حرف می باشد");
                }
            }
        };

        $scope.textArea_error = function (item) {
            if (item.data.isRequired == true &&
                (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')) {
                $scope.formErrors.unshift("فیلد " + item.data.title + " اجباری می باشد");
            }
            if (item.data.characterLimits == true) {
                if (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '') valide = false;
                else {
                    if (item.value.length < item.data.minCharacters)
                        $scope.formErrors.unshift("حداقل تعداد کاراکتر فیلد " + item.data.title + " " + item.data.minCharacters + "حرف می باشد");
                    if (item.value.length > item.data.maxCharacters)
                        $scope.formErrors.unshift("حداکثر تعداد کاراکتر فیلد " + item.data.title + " " + item.data.maxCharacters + "حرف می باشد");
                }
            }
        };

        $scope.numeric_error = function (item) {
            if (item.data.isRequired == true && isNaN(item.value) == true) {
                $scope.formErrors.unshift("فیلد " + item.data.title + " اجباری می باشد");
            }
            if (item.data.limits == true) {
                if (isNaN(item.value) == true) valide = false;
                else {
                    if (item.value < item.data.min)
                        $scope.formErrors.unshift("حداقل مقدار فیلد " + item.data.title + " " + item.data.min + " می باشد");
                    if (item.value > item.data.max)
                        $scope.formErrors.unshift("حداکثر مقدار فیلد " + item.data.title + " " + item.data.max + " می باشد");
                }
            }
        };

        $scope.colorPicker_error = function (item) {
            var re = /^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$/;
            if (item.data.isRequired == true) {
                if (item.value == '' || re.test(item.value) == false)
                    $scope.formErrors.unshift("فیلد " + item.data.title + " اجباری می باشد");
            }
            else {
                if (item.value != undefined && item.value != '' && re.test(item.value) == false)
                    $scope.formErrors.unshift("مقدار فیلد " + item.data.title + " صحیح می باشد");
            }
        };

        $scope.fileUploader_error = function (item) {
            if (item.data.isRequired == true &&
                (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')) {
                $scope.formErrors.unshift("فیلد " + item.data.title + " اجباری می باشد");
            }
        };

        $scope.webUrl_error = function (item) {
            var re = /^((http|https):\/\/)?[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:\/~+#-]*[\w@?^=%&amp;\/~+#-])?$/;
            if (item.data.isRequired == true) {
                if (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')
                    $scope.formErrors.unshift("فیلد " + item.data.title + " اجباری می باشد");
                else if (re.test(item.value) == false)
                    $scope.formErrors.unshift("مقدار فیلد " + item.data.title + " صحیح می باشد");
            }
            else {
                if (item.value != null && item.value != undefined && item.value != '' &&
                    item.value.trim() != '' && re.test(item.value) == false)
                    $scope.formErrors.unshift("مقدار فیلد " + item.data.title + " صحیح می باشد");
            }
        };

        $scope.datePicker_error = function (item) {
            if (item.data.isRequired == true &&
                (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')) {
                $scope.formErrors.unshift("فیلد " + item.data.title + " اجباری می باشد");
            }

            if (item.data.limits == true &&
                item.value != null &&
                item.value != undefined &&
                item.value != '' &&
                item.value.trim() != '') {
                var segments = item.value.split('/');
                var min = persianDate().subtract('days', item.data.min).hour(0).minute(0).seconds(0);
                var max = persianDate().add('days', item.data.max).hour(23).minute(59).seconds(59);
                var day = persianDate([Number(segments[0]), Number(segments[1]), Number(segments[2])]);

                if (min.toDate() > day.toDate()) {
                    $scope.formErrors.unshift("حداقل مقدار فیلد " + item.data.title + " " + item.data.min + " روز قبل می باشد");
                }

                if (max.toDate() < day.toDate()) {
                    $scope.formErrors.unshift("حداکثر مقدار فیلد " + item.data.title + " " + item.data.min + " روز بعد می باشد");
                }
            }
        };

        $scope.dropDown_error = function (item) {
            if (item.data.isRequired == true && item.value == null) {
                $scope.formErrors.unshift("فیلد " + item.data.title + " اجباری می باشد");
            }
        };

        $scope.multipleChoice_error = function (item) {
            if (item.data.isRequired == true && item.value == null) {
                $scope.formErrors.unshift("فیلد " + item.data.title + " اجباری می باشد");
            }
        };

        $scope.extendedFileUploader_error = function (item) {
            if (item.data.isRequired == true &&
                (item.value == null || item.value == undefined || item.value == '' || item.value.trim() == '')) {
                $scope.formErrors.unshift("فیلد " + item.data.title + " اجباری می باشد");
            }
        };

        /*=-=-=-=-= End Form Error =-=-=-=-=*/

        /*=-=-=-=-= Start Send Data =-=-=-=-=*/
        $scope.fromSumbited = false;

        $scope.send = function () {
            $scope.fromSumbited = true;
            if ($scope.validateForm() == false || $scope.uploadingFile > 0) return;

            $scope.addLoading = true;
            $http.post(baseUri + 'Order/Add',
            {
                formId: $scope.form.id,
                textBoxs: $scope.getFieldsValue(0),
                textAreas: $scope.getFieldsValue(1),
                numerics: $scope.getFieldsValue(2),
                colorPickers: $scope.getFieldsValue(3),
                fileUploaders: $scope.getFieldsValue(4),
                checkboxs: $scope.getFieldsValue(5),
                webUrls: $scope.getFieldsValue(6),
                datePickers: $scope.getFieldsValue(7),
                dropDowns: $scope.getFieldsValue(8),
                radioButtonGroups: $scope.getFieldsValue(9),
                checkBoxGroups: $scope.getFieldsValue(10),
                extendedFileUploaders: $scope.getFieldsValue(11),
                specialCreativity: $scope.specialCreativity,
                transportType: $scope.selectedTransport
            }).
            success(function (data, status, headers, config) {
                $scope.addLoading = false;
                if (data.Id != -1) {
                    $scope.resetForm();
                    $scope.showSaveMessage(data);
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.addLoading = false;
            });
        };

        $scope.getFieldsValue = function (type) {
            var filters = _.filter($scope.form.fields, function (item) { return item.type == type; });

            var values = _.map(filters, function (item) {
                switch (item.type) {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        return {
                            FieldId: item.data.id,
                            Value: item.value
                        };
                        break;

                    case 8:
                    case 9:
                        return {
                            FieldId: item.data.id,
                            Value: item.value.id
                        };
                        break;

                    case 10:
                        var filters = _.filter(item.data.items, function (val) { return val.value == true; });

                        return {
                            FieldId: item.data.id,
                            Values: _.map(filters, function (val) { return val.id; })
                        };
                        break;
                    case 11:
                        return {
                            FieldId: item.data.id,
                            Value: item.value,
                            Type: item.valueType
                        };
                        break;
                }
            });

            return values;
        };

        $scope.resetForm = function () {
            _.each($scope.form.fields, function (item) {
                switch (item.type) {
                    case 0:
                        item.value = '';
                        break;
                    case 1:
                        item.value = '';
                        break;
                    case 2:
                        item.value = '';
                        break;
                    case 3:
                        item.value = '';
                        break;
                    case 4:
                        item.value = '';
                        break;
                    case 5:
                        item.value = false;
                        break;
                    case 6:
                        item.value = '';
                        break;
                    case 7:
                        item.value = '';
                        break;
                    case 8:
                        item.value = null;
                        break;
                    case 9:
                        item.value = null;
                        break;
                    case 10:
                        _.each(item.data.items, function (val) {
                            val.value = false;
                        });
                        break;
                    case 11:
                        item.value = '';
                        item.valueText = '';
                        item.valueType = 0;
                        break;
                }
            });
            $scope.selectedTransport = $scope.transportType.none;
            $scope.fromSumbited = false;
            $scope.specialCreativity = false;
        };

        $scope.showSaveMessage = function (data) {
            if (data.Price == null) {
                ngDialog.open({
                    template: '<div style="font-family:yekan;font-size:14px;">' +
                        '<p>' + 'سفارش شما با موفقیت ثبت گردید' + '</p>' +
                        '<p>' + 'سفارش شما در انتظار تأیید و استعلام قیمت از طرف مدیریت می باشد' + '</p>' +
                        '<p>' + 'پس از تأیید جهت پرداخت به بخش صورت حساب مراجعه نمائید' + '</p>' +
                        '<div style="text-align:left;">' +
                        '<button type="button" class="btn btn-primary" ng-click="closeThisDialog(0)">خروج</button>' +
                        '</div></div>',
                    plain: true,
                    showClose: false
                });

                return;
            }

            if ($scope.isDesignPortal() == true) {
                var price = data.Price;
                var premayment = data.Prepayment;

                var priceStr = escape(price).replace(new RegExp(separator, "g"), "");
                var premaymentStr = escape(premayment).replace(new RegExp(separator, "g"), "");

                var separator = ",";
                var regexp = new RegExp("\\B(\\d{3})(" + separator + "|$)");

                do {
                    priceStr = priceStr.replace(regexp, separator + "$1");
                }while (priceStr.search(regexp) >= 0)

                do {
                    premaymentStr = premaymentStr.replace(regexp, separator + "$1");
                } while (premaymentStr.search(regexp) >= 0)

                ngDialog.open({
                    template: '<div style="font-family:yekan;font-size:14px;">' +
                          '<form>' +
                            '<p>' + 'سفارش شما با موفقیت ثبت گردید' + '</p>' +
                            '<div class="form-group">' +
                              '<div class="radio-inline c-radio">' +
                                '<label>' +
                                  '<input type="radio" ng-value="1" ng-model="paymentType" />' +
                                  '<span></span>' +
                                  'مبلغ پیش پرداخت ' + premaymentStr + ' ریال' +
                                '</label>' +
                              '</div>' +
                            '</div>' +
                            '<div class="form-group">' +
                              '<div class="radio-inline c-radio">' +
                                '<label>' +
                                  '<input type="radio" ng-value="2" ng-model="paymentType" />' +
                                  '<span></span>' +
                                  'مبلغ کل سفارش ' + priceStr + ' ریال' +
                                '</label>' +
                              '</div>' +
                            '</div>' +
                            '<div style="text-align:left;">' +
                              '<button class="btn btn-warning" ng-click="close()" type="button">خروج</button>' +
                              '<button class="btn btn-primary" ng-click="confirm()" type="button" style="margin-right:4px;">پرداخت</button>' +
                            '</div>' +
                          '</form>' +
                        '</div>',
                    plain: true,
                    showClose: false,
                    controller: ['$scope', 'ngDialog', function ($scope, ngDialog) {
                        $scope.paymentType = 1;

                        $scope.close = function () {
                            $scope.closeThisDialog({
                                response: 0,
                                type: 0
                            });
                        };
                        $scope.confirm = function () {
                            $scope.closeThisDialog({
                                response: 1,
                                type: $scope.paymentType
                            });
                        };
                    }],
                    preCloseCallback: function (value) {
                        if (value.response == 0) {
                            var nestedDialog = ngDialog.open({
                                template: '<div style="font-family:yekan;font-size:14px;">' +
                                    '<p>' + 'جهت پرداخت به بخش صورت حساب مراجعه فرمائید' + '</p>' +
                                    '<p>' + 'در صورت عدم پرداخت صورت حساب سفارش شما انجام نخواهد شد.' + '</p>' +
                                    '<div style="text-align:left;">' +
                                    '<button type="button" class="btn btn-primary" ng-click="closeThisDialog(2)">خروج</button>' +
                                    '</div></div>',
                                plain: true,
                                showClose: false
                            });

                            return nestedDialog;
                        }
                        else if (value.response == 1) {
                            if (value.type == 1) {
                                $state.go('^.payment-preview', { id: "p" + data.PrepaymentFactor.Id });
                            }
                            else if (value.type == 2) {
                                var id = [];
                                id.push("p" + data.PrepaymentFactor.Id);
                                id.push("f" + data.FinalFactor.Id);
                                $state.go('^.payment-preview', { id: id });
                            }

                            return true;
                        }
                    }
                });
            }

            else if ($scope.isPrintPortal() == true) {
                var price = data.Price;
                var printPrice = data.PrintPrice;
                var packingPrice = data.PackingPrice;

                var priceStr = escape(price).replace(new RegExp(separator, "g"), "");
                var printPriceStr = escape(printPrice).replace(new RegExp(separator, "g"), "");
                var packingPriceStr = escape(packingPrice).replace(new RegExp(separator, "g"), "");

                var separator = ",";
                var regexp = new RegExp("\\B(\\d{3})(" + separator + "|$)");

                do {
                    priceStr = priceStr.replace(regexp, separator + "$1");
                } while (priceStr.search(regexp) >= 0)

                do {
                    printPriceStr = printPriceStr.replace(regexp, separator + "$1");
                } while (printPriceStr.search(regexp) >= 0)

                do {
                    packingPriceStr = packingPriceStr.replace(regexp, separator + "$1");
                } while (packingPriceStr.search(regexp) >= 0)

                ngDialog.open({
                    template: '<div style="font-family:yekan;font-size:14px;">' +
                        '<p>' + 'سفارش شما با موفقیت ثبت گردید' + '</p>' +
                        '<p>' + 'مبلغ چاپ ' + printPriceStr + ' ریال' + '</p>' +
                        '<p>' + 'مبلغ بسته بندی ' + packingPriceStr + ' ریال' + '</p>' +
                        '<div style="text-align:left;">' +
                        '<button class="btn btn-warning" ng-click="close()" type="button">خروج</button>' +
                        '<button class="btn btn-primary" ng-click="confirm()" type="button" style="margin-right:4px;">پرداخت</button>' +
                        '</div></div>',
                    plain: true,
                    showClose: false,
                    controller: ['$scope', 'ngDialog', function ($scope, ngDialog) {
                        $scope.close = function () {
                            $scope.closeThisDialog(0);
                        };
                        $scope.confirm = function () {
                            $scope.closeThisDialog(1);
                        };
                    }],
                    preCloseCallback: function (value) {
                        if (value == 0) {
                            var nestedDialog = ngDialog.open({
                                template: '<div style="font-family:yekan;font-size:14px;">' +
                                    '<p>' + 'جهت پرداخت به بخش صورت حساب مراجعه فرمائید' + '</p>' +
                                    '<p>' + 'در صورت عدم پرداخت صورت حساب سفارش شما انجام نخواهد شد.' + '</p>' +
                                    '<div style="text-align:left;">' +
                                    '<button type="button" class="btn btn-primary" ng-click="closeThisDialog(2)">خروج</button>' +
                                    '</div></div>',
                                plain: true,
                                showClose: false
                            });

                            return nestedDialog;
                        }
                        else if (value == 1) {
                            $state.go('^.payment-preview', { id: data.Factor.Id });

                            return true;
                        }
                    }
                });
            }
        };
        /*=-=-=-=-= End Send Data =-=-=-=-=*/

        /*=-=-=-=-= Start Manage Preview =-=-=-=-=*/
        $scope.gridsterOpts_desktop = {
            minRows: 2,
            maxRows: 2500,
            columns: 3,
            colWidth: 'auto',
            rowHeight: 10,
            margins: [0, 0],
            defaultSizeX: 1,
            defaultSizeY: 1,
            mobileBreakPoint: 600,
            resizable: { enabled: false },
            draggable: { enabled: false }
        };

        $scope.gridsterOpts_tablet = {
            minRows: 2,
            maxRows: 2500,
            columns: 2,
            colWidth: 'auto',
            rowHeight: 10,
            margins: [0, 0],
            defaultSizeX: 1,
            defaultSizeY: 1,
            mobileBreakPoint: 600,
            resizable: { enabled: false },
            draggable: { enabled: false }
        };

        $scope.gridsterOpts_mobile = {
            minRows: 2,
            maxRows: 2500,
            columns: 1,
            colWidth: 'auto',
            rowHeight: 10,
            margins: [0, 0],
            defaultSizeX: 1,
            defaultSizeY: 1,
            mobileBreakPoint: 200,
            resizable: { enabled: false },
            draggable: { enabled: false }
        };
        /*=-=-=-=-= End Manage Preview =-=-=-=-=*/
    }]);