/**=========================================================
 * Module: order-add.js
 * Send New Order
 =========================================================*/

App.controller('AddOrderController', ['$scope', '$http', 'APP_BASE_URI', '$upload', 'toaster',
    function ($scope, $http, baseUri, $upload, toaster) {

        /*=-=-=-=-= Start Fetch Forms =-=-=-=-=*/
        $scope.forms = [];
        $scope.fetchForms = function () {
            $scope.fetchFormsLoading = true;

            $http.get(baseUri + 'Form/Get', {
                params: {
                    isDesignForm: $scope.isDesignPortal(),
                    isPrintForm: $scope.isPrintPortal()
                }
            })
            .success(function (data, status, headers, config) {
                $scope.forms = data.Data;
                $scope.fetchFormsLoading = false;
                setTimeout(function () { $scope.formItemShowAnimate(0); }, 150);
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
        $scope.fetchForms();
        /*=-=-=-=-= End Fetch Forms =-=-=-=-=*/

        /*=-=-=-=-= Start Select Form =-=-=-=-=*/
        $scope.selectedForm = null;

        $scope.selectForm = function ($event, form) {
            $scope.selectedForm = form;
            $('.forms-list li').fadeOut(300, function () {
                $scope.fetchFields();
            });
        };

        $scope.unSelectForm = function ($event, form) {
            $scope.selectedForm = null;
            $scope.form = {};
            $scope.fromSumbited = false;
            $('.forms-list li').fadeIn(300);
        };

        $scope.formItemHideAnimate = function (index) {
            var items = $('.forms-list li');
            $(items[index]).fadeOut(200, function () {
                if (index > 0) {
                    $scope.formItemHideAnimate(--index);
                }
                else {
                    $scope.fetchFields();
                }
            });
        };

        $scope.formItemShowAnimate = function (index) {
            var items = $('.forms-list li');
            $(items[index]).fadeIn(200, function () {
                if (index < items.length - 1) {
                    $scope.formItemShowAnimate(++index);
                }
            });
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
                }
            });

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
                if (item.value != '' && re.test(item.value) == false)
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

        /*=-=-=-=-= End Validate Field =-=-=-=-=*/

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
                checkBoxGroups: $scope.getFieldsValue(10)
            }).
            success(function (data, status, headers, config) {
                $scope.addLoading = false;
                if (data.Id != -1) {
                    toaster.pop('success', "اطلاعات با موفقیت ثبت گردید");
                    $scope.resetForm();
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
                }
            });

            return values;
        };
        /*=-=-=-=-= End Send Data =-=-=-=-=*/

        /*=-=-=-=-= Start Manage Preview =-=-=-=-=*/
        $scope.gridsterOpts_desktop = {
            minRows: 2,
            maxRows: 100,
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
            maxRows: 100,
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
            maxRows: 100,
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