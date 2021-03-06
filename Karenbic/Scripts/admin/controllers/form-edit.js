﻿/**=========================================================
 * Module: form-edit.js
 * Edit Form
 =========================================================*/

App.controller('EditFormController', ['$scope', '$http', 'ngDialog', '$modal', 'APP_BASE_URI', '$upload', 'toaster', '$stateParams',
    function ($scope, $http, ngDialog, $modal, baseUri, $upload, toaster, $stateParams) {

        /*=-=-=-=-= Start Define Variable =-=-=-=-=*/
        $scope.groups = [];

        //Form Fields Type
        $scope.formFieldTypes = [
            {
                text: "TextBox",
                value: 0,
                imageSrc: "/Images/FormField/text-box.png"
            },
            {
                text: "TextArea",
                value: 1,
                imageSrc: "/Images/FormField/text-area.png"
            },
            {
                text: "Numeric Stepper",
                value: 2,
                imageSrc: "/Images/FormField/numeric.png"
            },
            {
                text: "Color Picker",
                value: 3,
                imageSrc: "/Images/FormField/color-picker.png"
            },
            {
                text: "File Uploader",
                value: 4,
                imageSrc: "/Images/FormField/file-uploader.png"
            },
            {
                text: "Checkbox",
                value: 5,
                imageSrc: "/Images/FormField/check-box.png"
            },
            {
                text: "Web Url",
                value: 6,
                imageSrc: "/Images/FormField/web-url.png"
            },
            {
                text: "Date Picker",
                value: 7,
                imageSrc: "/Images/FormField/date-picker.png"
            },
            {
                text: "Drop Down",
                value: 8,
                imageSrc: "/Images/FormField/drop-down.png"
            },
            {
                text: "Multiple Choice",
                value: 9,
                imageSrc: "/Images/FormField/multiple-choice.png"
            },
            {
                text: "Checkbox Group",
                value: 10,
                imageSrc: "/Images/FormField/multiple-choice.png"
            },
            {
                text: "Extended File Uploader",
                value: 11,
                description: "",
                imageSrc: "/Images/FormField/file-uploader.png"
            },
            {
                text: "Label",
                value: 12,
                description: "",
                imageSrc: "/Images/FormField/label.png"
            }
        ];

        //File Formats
        $scope.fileFormats = [
            {
                Id: 1,
                Title: 'JPG',
                Extention: 'jpg'
            },
            {
                Id: 2,
                Title: 'JPEG',
                Extention: 'jpeg'
            },
            {
                Id: 3,
                Title: 'PNG',
                Extention: 'png'
            },
            {
                Id: 4,
                Title: 'GIF',
                Extention: 'gif'
            },
            {
                Id: 5,
                Title: 'TIFF',
                Extention: 'tiff'
            },
            {
                Id: 6,
                Title: 'PDF',
                Extention: 'pdf'
            },
            {
                Id: 7,
                Title: 'TXT',
                Extention: 'txt'
            },
            {
                Id: 8,
                Title: 'DOC',
                Extention: 'doc'
            },
            {
                Id: 9,
                Title: 'DOCX',
                Extention: 'docx'
            },
            {
                Id: 10,
                Title: 'ZIP',
                Extention: 'zip'
            },
            {
                Id: 11,
                Title: 'RAR',
                Extention: 'rar'
            }
        ];

        //Fonts
        $scope.fonts = [
            {
                Id: 1,
                Title: 'Arial'
            },
            {
                Id: 2,
                Title: 'BNAZANB'
            },
            {
                Id: 3,
                Title: 'BNazanin'
            },
            {
                Id: 4,
                Title: 'IranianSerifWeb'
            },
            {
                Id: 5,
                Title: 'NexaBold'
            },
            {
                Id: 6,
                Title: 'Tahoma'
            },
            {
                Id: 7,
                Title: 'yekan'
            }
        ];
        /*=-=-=-=-= End Define Variable =-=-=-=-=*/

        //Fetch Form Groups
        $scope.fetchGroups = function () {
            $scope.fetchGroupsLoading = true;

            $http.get(baseUri + 'FormGroup/Get', {
                params: {
                    portal: $scope.isDesignPortal() == true ? 1 : 2
                }
            })
            .success(function (data, status, headers, config) {
                $scope.groups = data.Data;
                
                var index = _.findIndex($scope.groups, function (item) {
                    return item.Id == $scope.form.group.Id;
                });

                $scope.form.group = $scope.groups[index];

                $scope.fetchGroupsLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchGroupsLoading = false;
            });
        };

        //New Field Type
        $scope.formFieldType = $scope.formFieldTypes[0];
        $scope.$watch(function () {
            return $scope.formFieldType;
        }, function (newValue, oldValue) {
            //Reset Picture File
            $scope.PictureHelpFile_Reset();

            //Reset Fields
            $scope.newField_TextBox_Reset();
            $scope.newField_TextArea_Reset();
            $scope.newField_NumericStepper_Reset();
            $scope.newField_ColorPicker_Reset();
            $scope.newField_FileUploader_Reset();
            $scope.newField_Checkbox_Reset();
            $scope.newField_WebUrl_Reset();
            $scope.newField_DatePicker_Reset();
            $scope.newField_DropDown_Reset();
            $scope.newField_MultipleChoice_Reset();
            $scope.newField_MultipleChoice_Reset();
            $scope.newField_FileUploader2_Reset();
            $scope.newField_Label_Reset();
        });

        /*=-=-=-=-= Start Fetch Data Form =-=-=-=-=*/
        $scope.fetchForm = function () {
            if ($stateParams.id == null || isNaN($stateParams.id) == true) return;

            $scope.fetchLoading = true;
            $http.get(baseUri + 'Form/Find', {
                params: {
                    id: $stateParams.id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.form = data.Data;
                $scope.fetchForm_FixLabel();
                $scope.fetchGroups();
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
        $scope.fetchForm_FixLabel = function () {
            var labels = _.where($scope.form.fields, { type: 12 });

            _.each(labels, function(label) {
                var font = _.clone(label.data.font);
                var fontIndex = _.findIndex($scope.fonts, function (item) {
                    return item.Title == font;
                });

                label.data.font = $scope.fonts[fontIndex];
            });
        };
        $scope.fetchForm();
        /*=-=-=-=-= End Fetch Data Form =-=-=-=-=*/

        /*=-=-=-=-= Start Help File =-=-=-=-=*/
        $scope.PictureHelpFile = null;

        $scope.PictureHelpFile_Reset = function () {
            $scope.PictureHelpFile = null;
            $('#pictureHelpFileInput').val('');
            $('#pictureHelpFileInput').closest('.form-group').find('input[type=text]').val('');
        };

        $scope.addPictureHelpFile = function ($files) {
            var $file = $files[0];

            if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                return;
            }

            if ($file.size > 150 * 1024) {
                toaster.pop('error', "حداکثر حجم فایل 150 کیلو بایت می باشد");
                return;
            }

            $scope.PictureHelpFile = $file;
        };
        /*=-=-=-=-= End Help File =-=-=-=-=*/

        /*=-=-=-=-= Start New TextBox =-=-=-=-=*/
        $scope.newField_TextBox = {
            title: '',
            defualt: '',
            isRequired: true,
            characterLimits: false,
            minCharacters: 0,
            maxCharacters: 0,
            showInFactor: false,
            description: '',
            showCustomer: true,
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0
        };

        $scope.$watch(function () {
            return $scope.newField_TextBox.minCharacters;
        }, function (newValue, oldValue) {
            if (newValue > $scope.newField_TextBox.maxCharacters) {
                $scope.newField_TextBox.maxCharacters = newValue;
            }
        });

        $scope.$watch(function () {
            return $scope.newField_TextBox.maxCharacters;
        }, function (newValue, oldValue) {
            if (newValue < $scope.newField_TextBox.minCharacters) {
                $scope.newField_TextBox.minCharacters = newValue;
            }
        });

        $scope.newField_TextBox_Reset = function () {
            $scope.newField_TextBox = {
                title: '',
                defualt: '',
                isRequired: true,
                characterLimits: false,
                minCharacters: 0,
                maxCharacters: 0,
                showInFactor: false,
                description: '',
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New TextBox =-=-=-=-=*/

        /*=-=-=-=-= Start New TextArea =-=-=-=-=*/
        $scope.newField_TextArea = {
            title: '',
            isRequired: true,
            characterLimits: false,
            minCharacters: 0,
            maxCharacters: 0,
            showInFactor: false,
            description: '',
            height: 120,
            showCustomer: true,
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0
        };

        $scope.$watch(function () {
            return $scope.newField_TextArea.minCharacters;
        }, function (newValue, oldValue) {
            if (newValue > $scope.newField_TextArea.maxCharacters) {
                $scope.newField_TextArea.maxCharacters = newValue;
            }
        });

        $scope.$watch(function () {
            return $scope.newField_TextArea.maxCharacters;
        }, function (newValue, oldValue) {
            if (newValue < $scope.newField_TextArea.minCharacters) {
                $scope.newField_TextArea.minCharacters = newValue;
            }
        });

        $scope.newField_TextArea_Reset = function () {
            $scope.newField_TextArea = {
                title: '',
                isRequired: true,
                characterLimits: false,
                minCharacters: 0,
                maxCharacters: 0,
                showInFactor: false,
                description: '',
                height: 120,
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New TextArea =-=-=-=-=*/

        /*=-=-=-=-= Start New Numeric Stepper =-=-=-=-=*/
        $scope.newField_NumericStepper = {
            title: '',
            isInt: true,
            defualt: 0,
            isRequired: true,
            limits: false,
            min: 0,
            max: 0,
            showInFactor: false,
            description: '',
            showCustomer: true,
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0,
            useForPrice: false
        };

        $scope.$watch(function () {
            return $scope.newField_NumericStepper.isInt;
        }, function (newValue, oldValue) {
            if (newValue == true) {
                if ($scope.newField_NumericStepper.defualt % 1 !== 0) {
                    $scope.newField_NumericStepper.defualt = Math.round($scope.newField_NumericStepper.defualt);
                }
                if ($scope.newField_NumericStepper.min % 1 !== 0) {
                    $scope.newField_NumericStepper.min = Math.round($scope.newField_NumericStepper.min);
                }
                if ($scope.newField_NumericStepper.max % 1 !== 0) {
                    $scope.newField_NumericStepper.max = Math.round($scope.newField_NumericStepper.max);
                }
            }
        });

        $scope.$watch(function () {
            return $scope.newField_NumericStepper.min;
        }, function (newValue, oldValue) {
            if (newValue > $scope.newField_NumericStepper.max) {
                $scope.newField_NumericStepper.max = newValue;
            }
        });

        $scope.$watch(function () {
            return $scope.newField_NumericStepper.max;
        }, function (newValue, oldValue) {
            if (newValue < $scope.newField_NumericStepper.min) {
                $scope.newField_NumericStepper.min = newValue;
            }
        });

        $scope.newField_NumericStepper_Reset = function () {
            $scope.newField_NumericStepper = {
                title: '',
                isInt: true,
                defualt: 0,
                isRequired: true,
                limits: false,
                min: 0,
                max: 0,
                showInFactor: false,
                description: '',
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New Numeric Stepper =-=-=-=-=*/

        /*=-=-=-=-= Start New Color Picker =-=-=-=-=*/
        $scope.newField_ColorPicker = {
            title: '',
            isRequired: true,
            description: '',
            showCustomer: true,
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0
        };

        $scope.newField_ColorPicker_Reset = function () {
            $scope.newField_ColorPicker = {
                title: '',
                isRequired: true,
                description: '',
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New Color Picker =-=-=-=-=*/

        /*=-=-=-=-= Start New File Uploader =-=-=-=-=*/
        $scope.newField_FileUploader = {
            title: '',
            fileTypes: [],
            isRequired: true,
            sizeLimits: false,
            minSize: 0,
            maxSize: 0,
            description: '',
            showCustomer: true,
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0
        };

        $scope.$watch(function () {
            return $scope.newField_FileUploader.minSize;
        }, function (newValue, oldValue) {
            if (newValue > $scope.newField_FileUploader.maxSize) {
                $scope.newField_FileUploader.maxSize = newValue;
            }
        });

        $scope.$watch(function () {
            return $scope.newField_FileUploader.maxSize;
        }, function (newValue, oldValue) {
            if (newValue < $scope.newField_FileUploader.minSize) {
                $scope.newField_FileUploader.minSize = newValue;
            }
        });

        $scope.newField_FileUploader_Reset = function () {
            $scope.newField_FileUploader = {
                title: '',
                fileTypes: [],
                isRequired: true,
                sizeLimits: false,
                minSize: 0,
                maxSize: 0,
                description: '',
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New File Uploader =-=-=-=-=*/

        /*=-=-=-=-= Start New Checkbox =-=-=-=-=*/
        $scope.newField_Checkbox = {
            title: '',
            showInFactor: false,
            description: '',
            showCustomer: true,
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0,
            useForPrice: false
        };

        $scope.newField_Checkbox_Reset = function () {
            $scope.newField_Checkbox = {
                title: '',
                showInFactor: false,
                description: '',
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New Checkbox =-=-=-=-=*/

        /*=-=-=-=-= Start New Web Url =-=-=-=-=*/
        $scope.newField_WebUrl = {
            title: '',
            isRequired: true,
            description: '',
            showCustomer: true,
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0
        };

        $scope.newField_WebUrl_Reset = function () {
            $scope.newField_WebUrl = {
                title: '',
                isRequired: true,
                description: '',
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New Web Url =-=-=-=-=*/

        /*=-=-=-=-= Start New Date Picker =-=-=-=-=*/
        $scope.newField_DatePicker = {
            title: '',
            isRequired: true,
            limits: false,
            min: 0,
            max: 0,
            showInFactor: false,
            description: '',
            showCustomer: true,
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0
        };

        $scope.newField_DatePicker_Reset = function () {
            $scope.newField_DatePicker = {
                title: '',
                isInt: true,
                defualt: 0,
                isRequired: true,
                limits: false,
                min: 0,
                max: 0,
                showInFactor: false,
                description: '',
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New Date Picker =-=-=-=-=*/

        /*=-=-=-=-= Start New Drop Down =-=-=-=-=*/
        $scope.newField_DropDown_NewItem = {
            title: ''
        };

        $scope.newField_DropDown = {
            title: '',
            isRequired: true,
            showInFactor: false,
            description: '',
            showCustomer: true,
            items: [],
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0,
            useForPrice: false
        };

        $scope.newField_DropDown_AddItem = function () {
            if ($scope.newField_DropDown_NewItem.title == null || $scope.newField_DropDown_NewItem.title == '') return;
            $scope.newField_DropDown.items.push({
                title: $scope.newField_DropDown_NewItem.title
            });
            $scope.newField_DropDown_NewItem.title = '';
        };

        $scope.newField_DropDown_MoveUpItem = function (index) {
            if ($scope.newField_DropDown.items.length > 1 && index > 0) {
                var temp = _.clone($scope.newField_DropDown.items[index]);
                $scope.newField_DropDown.items[index] = _.clone($scope.newField_DropDown.items[index - 1]);
                $scope.newField_DropDown.items[index - 1] = _.clone(temp);
            }
        };

        $scope.newField_DropDown_MoveDownItem = function (index) {
            if ($scope.newField_DropDown.items.length > 1 && index < $scope.newField_DropDown.items.length - 1) {
                var temp = _.clone($scope.newField_DropDown.items[index]);
                $scope.newField_DropDown.items[index] = _.clone($scope.newField_DropDown.items[index + 1]);
                $scope.newField_DropDown.items[index + 1] = _.clone(temp);
            }
        };

        $scope.newField_DropDown_RemoveItem = function (index) {
            $scope.newField_DropDown.items.splice(index, 1);
        };

        $scope.newField_DropDown_Reset = function () {
            $scope.newField_DropDown = {
                title: '',
                isRequired: true,
                showInFactor: false,
                description: '',
                items: [],
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New Drop Down =-=-=-=-=*/

        /*=-=-=-=-= Start New Multiple Choice =-=-=-=-=*/
        $scope.newField_MultipleChoice_NewItem = {
            title: ''
        };

        $scope.newField_MultipleChoice = {
            title: '',
            isRequired: true,
            showInFactor: false,
            description: '',
            showCustomer: true,
            items: [],
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0,
            useForPrice: false
        };

        $scope.newField_MultipleChoice_AddItem = function () {
            if ($scope.newField_MultipleChoice_NewItem.title == null || $scope.newField_MultipleChoice_NewItem.title == '') return;
            $scope.newField_MultipleChoice.items.push({
                title: $scope.newField_MultipleChoice_NewItem.title,
                hasPictureHelpFile: false,
                pictureHelpFile: ''
            });
            $scope.newField_MultipleChoice_NewItem.title = '';
        };

        $scope.newField_MultipleChoice_MoveUpItem = function (index) {
            if ($scope.newField_MultipleChoice.items.length > 1 && index > 0) {
                var temp = _.clone($scope.newField_MultipleChoice.items[index]);
                $scope.newField_MultipleChoice.items[index] = _.clone($scope.newField_MultipleChoice.items[index - 1]);
                $scope.newField_MultipleChoice.items[index - 1] = _.clone(temp);
            }
        };

        $scope.newField_MultipleChoice_MoveDownItem = function (index) {
            if ($scope.newField_MultipleChoice.items.length > 1 && index < $scope.newField_MultipleChoice.items.length - 1) {
                var temp = _.clone($scope.newField_MultipleChoice.items[index]);
                $scope.newField_MultipleChoice.items[index] = _.clone($scope.newField_MultipleChoice.items[index + 1]);
                $scope.newField_MultipleChoice.items[index + 1] = _.clone(temp);
            }
        };

        $scope.newField_MultipleChoice_RemoveItem = function (index) {
            $scope.newField_MultipleChoice.items.splice(index, 1);
        };

        $scope.newField_MultipleChoice_PictureHelpFile = function (item, $event) {
            if (item.hasPictureHelpFile == false) {
                $($event.currentTarget).closest('td').find('input[type=file]').click();
            }
            else {
                ngDialog.open({
                    template: '<div style="font-family:yekan;font-size:14px;">' +
                          '<form>' +
                            '<div style="text-align:center;">' +
                              '<button class="btn btn-lg btn-warning" ng-click="remove()" type="button">حذف فایل راهنما</button>' +
                              '<button class="btn btn-lg btn-primary" ng-click="reselect()" type="button" style="margin-right:4px;">انتخاب مجدد فایل راهنما</button>' +
                            '</div>' +
                          '</form>' +
                        '</div>',
                    plain: true,
                    showClose: false,
                    className: 'ngdialog-theme-default ngdialog-theme1',
                    controller: ['$scope', 'ngDialog', function ($scope, ngDialog) {
                        $scope.paymentType = 1;

                        $scope.remove = function () {
                            $scope.closeThisDialog({
                                response: 0,
                                type: 0
                            });
                        };
                        $scope.reselect = function () {
                            $scope.closeThisDialog({
                                response: 1,
                                type: $scope.paymentType
                            });
                        };
                    }],
                    preCloseCallback: function (value) {
                        if (value.response == 0) {
                            $scope.newField_MultipleChoice_RemovePictureHelpFile(item);
                        }
                        else if (value.response == 1) {
                            $($event.currentTarget).closest('td').find('input[type=file]').click();
                        }
                        return true;
                    }
                });
            }
        };

        $scope.newField_MultipleChoice_AddPictureHelpFile = function (item, $files) {
            var $file = $files[0];

            if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                return;
            }

            if ($file.size > 150 * 1024) {
                toaster.pop('error', "حداکثر حجم فایل 150 کیلو بایت می باشد");
                return;
            }

            item.uploadingPictureHelpFile = true;

            $upload.upload({
                url: baseUri + 'Form/UploadPicture',
                file: $file
            }).success(function (data, status, headers, config) {
                item.pictureHelpFile = data;
                item.hasPictureHelpFile = true;
                item.uploadingPictureHelpFile = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                item.uploadingPictureHelpFile = false;
            });
        };

        $scope.newField_MultipleChoice_RemovePictureHelpFile = function (item) {
            item.pictureHelpFile = '';
            item.hasPictureHelpFile = false;
        };

        $scope.newField_MultipleChoice_Reset = function () {
            $scope.newField_MultipleChoice = {
                title: '',
                isRequired: true,
                showInFactor: false,
                description: '',
                items: [],
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New Multiple Choice =-=-=-=-=*/

        /*=-=-=-=-= Start New Checkbox Group =-=-=-=-=*/
        $scope.newField_CheckboxGroup_NewItem = {
            title: ''
        };

        $scope.newField_CheckboxGroup = {
            title: '',
            showInFactor: false,
            description: '',
            showCustomer: true,
            items: [],
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0,
            useForPrice: false
        };

        $scope.newField_CheckboxGroup_AddItem = function () {
            if ($scope.newField_CheckboxGroup_NewItem.title == null || $scope.newField_CheckboxGroup_NewItem.title == '') return;
            $scope.newField_CheckboxGroup.items.push({
                title: $scope.newField_CheckboxGroup_NewItem.title,
                hasPictureHelpFile: false,
                pictureHelpFile: ''
            });
            $scope.newField_CheckboxGroup_NewItem.title = '';
        };

        $scope.newField_CheckboxGroup_MoveUpItem = function (index) {
            if ($scope.newField_CheckboxGroup.items.length > 1 && index > 0) {
                var temp = _.clone($scope.newField_CheckboxGroup.items[index]);
                $scope.newField_CheckboxGroup.items[index] = _.clone($scope.newField_CheckboxGroup.items[index - 1]);
                $scope.newField_CheckboxGroup.items[index - 1] = _.clone(temp);
            }
        };

        $scope.newField_CheckboxGroup_MoveDownItem = function (index) {
            if ($scope.newField_CheckboxGroup.items.length > 1 && index < $scope.newField_CheckboxGroup.items.length - 1) {
                var temp = _.clone($scope.newField_CheckboxGroup.items[index]);
                $scope.newField_CheckboxGroup.items[index] = _.clone($scope.newField_CheckboxGroup.items[index + 1]);
                $scope.newField_CheckboxGroup.items[index + 1] = _.clone(temp);
            }
        };

        $scope.newField_CheckboxGroup_RemoveItem = function (index) {
            $scope.newField_CheckboxGroup.items.splice(index, 1);
        };

        $scope.newField_CheckboxGroup_PictureHelpFile = function (item, $event) {
            if (item.hasPictureHelpFile == false) {
                $($event.currentTarget).closest('td').find('input[type=file]').click();
            }
            else {
                ngDialog.open({
                    template: '<div style="font-family:yekan;font-size:14px;">' +
                          '<form>' +
                            '<div style="text-align:center;">' +
                              '<button class="btn btn-lg btn-warning" ng-click="remove()" type="button">حذف فایل راهنما</button>' +
                              '<button class="btn btn-lg btn-primary" ng-click="reselect()" type="button" style="margin-right:4px;">انتخاب مجدد فایل راهنما</button>' +
                            '</div>' +
                          '</form>' +
                        '</div>',
                    plain: true,
                    showClose: false,
                    className: 'ngdialog-theme-default ngdialog-theme1',
                    controller: ['$scope', 'ngDialog', function ($scope, ngDialog) {
                        $scope.paymentType = 1;

                        $scope.remove = function () {
                            $scope.closeThisDialog({
                                response: 0,
                                type: 0
                            });
                        };
                        $scope.reselect = function () {
                            $scope.closeThisDialog({
                                response: 1,
                                type: $scope.paymentType
                            });
                        };
                    }],
                    preCloseCallback: function (value) {
                        if (value.response == 0) {
                            $scope.newField_CheckboxGroup_RemovePictureHelpFile(item);
                        }
                        else if (value.response == 1) {
                            $($event.currentTarget).closest('td').find('input[type=file]').click();
                        }
                        return true;
                    }
                });
            }
        };

        $scope.newField_CheckboxGroup_AddPictureHelpFile = function (item, $files) {
            var $file = $files[0];

            if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                return;
            }

            if ($file.size > 150 * 1024) {
                toaster.pop('error', "حداکثر حجم فایل 150 کیلو بایت می باشد");
                return;
            }

            item.uploadingPictureHelpFile = true;

            $upload.upload({
                url: baseUri + 'Form/UploadPicture',
                file: $file
            }).success(function (data, status, headers, config) {
                item.pictureHelpFile = data;
                item.hasPictureHelpFile = true;
                item.uploadingPictureHelpFile = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                item.uploadingPictureHelpFile = false;
            });
        };

        $scope.newField_CheckboxGroup_RemovePictureHelpFile = function (item) {
            item.pictureHelpFile = '';
            item.hasPictureHelpFile = false;
        };

        $scope.newField_CheckboxGroup_Reset = function () {
            $scope.newField_CheckboxGroup = {
                title: '',
                isRequired: true,
                showInFactor: false,
                description: '',
                items: [],
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New Checkbox Group =-=-=-=-=*/

        /*=-=-=-=-= Start New File Uploader 2 =-=-=-=-=*/
        $scope.newField_FileUploader2 = {
            title: '',
            fileTypes: [],
            isRequired: true,
            sizeLimits: false,
            minSize: 0,
            maxSize: 0,
            description: '',
            showCustomer: true,
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            priority: 0
        };

        $scope.$watch(function () {
            return $scope.newField_FileUploader2.minSize;
        }, function (newValue, oldValue) {
            if (newValue > $scope.newField_FileUploader2.maxSize) {
                $scope.newField_FileUploader2.maxSize = newValue;
            }
        });

        $scope.$watch(function () {
            return $scope.newField_FileUploader2.maxSize;
        }, function (newValue, oldValue) {
            if (newValue < $scope.newField_FileUploader2.minSize) {
                $scope.newField_FileUploader2.minSize = newValue;
            }
        });

        $scope.newField_FileUploader2_Reset = function () {
            $scope.newField_FileUploader2 = {
                title: '',
                fileTypes: [],
                isRequired: true,
                sizeLimits: false,
                minSize: 0,
                maxSize: 0,
                description: '',
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                priority: 0
            };
        };
        /*=-=-=-=-= End New File Uploader 2 =-=-=-=-=*/

        /*=-=-=-=-= Start New Label =-=-=-=-=*/
        $scope.newField_Label = {
            title: '',
            color: '',
            font: $scope.fonts[6],
            size: 11,
            underline: false,
            upline: false,
            pictureHelpFile: '',
            pictureHelpPath: '',
            hasPictureHelpFile: false,
            description: '',
            priority: 0
        };

        $scope.newField_Label_Reset = function () {
            $scope.newField_Label = {
                title: '',
                color: '',
                font: $scope.fonts[6],
                size: 11,
                underline: false,
                upline: false,
                pictureHelpFile: '',
                pictureHelpPath: '',
                hasPictureHelpFile: false,
                description: '',
                priority: 0
            };
        };
        /*=-=-=-=-= End New Label =-=-=-=-=*/

        /*=-=-=-=-= Start Manage Fields =-=-=-=-=*/
        $scope.removedFields = [];

        $scope.addField_step1 = function () {
            if ($scope.formFieldType == null) return;
            if ($scope.addFieldForm.$invalid) return;

            if ($scope.PictureHelpFile != null) {
                $scope.addFieldLoading = true;
                $upload.upload({
                    url: baseUri + 'Form/UploadPicture',
                    file: $scope.PictureHelpFile
                }).success(function (data, status, headers, config) {
                    $scope.addField_step2(data);
                    $scope.PictureHelpFile_Reset();
                    $scope.addFieldFromSubmited = false;
                    $scope.addFieldLoading = false;
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                    }
                    $scope.addFieldLoading = false;
                });
            }
            else {
                $scope.addFieldLoading = true;
                $scope.addField_step2('');
                $scope.addFieldLoading = false;
                $scope.addFieldFromSubmited = false;
            }
        };

        $scope.addField_step2 = function (pictureHelpFile) {
            switch ($scope.formFieldType.value) {
                case 0:
                    if (pictureHelpFile != '') {
                        $scope.newField_TextBox.pictureHelpFile = pictureHelpFile;
                        $scope.newField_TextBox.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_TextBox.hasPictureHelpFile = true;
                    }

                    var obj = {
                        isNew: true,
                        type: 0,
                        data: _.clone($scope.newField_TextBox),
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 7;
                        obj.tablet_position.sizeY = 7;
                        obj.mobile_position.sizeY = 7;
                    }
                    else {
                        obj.desktop_position.sizeY = 9;
                        obj.tablet_position.sizeY = 9;
                        obj.mobile_position.sizeY = 9;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_TextBox_Reset();

                    break;

                case 1:
                    if (pictureHelpFile != '') {
                        $scope.newField_TextArea.pictureHelpFile = pictureHelpFile;
                        $scope.newField_TextArea.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_TextArea.hasPictureHelpFile = true;
                    }

                    var obj = {
                        isNew: true,
                        type: 1,
                        data: _.clone($scope.newField_TextArea),
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 2,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 2,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 2,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 5 + (obj.data.height / 10);
                        obj.tablet_position.sizeY = 5 + (obj.data.height / 10);
                        obj.mobile_position.sizeY = 5 + (obj.data.height / 10);
                    }
                    else {
                        obj.desktop_position.sizeY = 7 + (obj.data.height / 10);
                        obj.tablet_position.sizeY = 7 + (obj.data.height / 10);
                        obj.mobile_position.sizeY = 7 + (obj.data.height / 10);
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_TextArea_Reset();

                    break;

                case 2:
                    if (pictureHelpFile != '') {
                        $scope.newField_NumericStepper.pictureHelpFile = pictureHelpFile;
                        $scope.newField_NumericStepper.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_NumericStepper.hasPictureHelpFile = true;
                    }

                    var obj = {
                        isNew: true,
                        type: 2,
                        data: _.clone($scope.newField_NumericStepper),
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 7;
                        obj.tablet_position.sizeY = 7;
                        obj.mobile_position.sizeY = 7;
                    }
                    else {
                        obj.desktop_position.sizeY = 9;
                        obj.tablet_position.sizeY = 9;
                        obj.mobile_position.sizeY = 9;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_NumericStepper_Reset();

                    break;

                case 3:
                    if (pictureHelpFile != '') {
                        $scope.newField_ColorPicker.pictureHelpFile = pictureHelpFile;
                        $scope.newField_ColorPicker.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_ColorPicker.hasPictureHelpFile = true;
                    }

                    var obj = {
                        isNew: true,
                        type: 3,
                        data: _.clone($scope.newField_ColorPicker),
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 7;
                        obj.tablet_position.sizeY = 7;
                        obj.mobile_position.sizeY = 7;
                    }
                    else {
                        obj.desktop_position.sizeY = 9;
                        obj.tablet_position.sizeY = 9;
                        obj.mobile_position.sizeY = 9;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_ColorPicker_Reset();

                    break;

                case 4:
                    if (pictureHelpFile != '') {
                        $scope.newField_FileUploader.pictureHelpFile = pictureHelpFile;
                        $scope.newField_FileUploader.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_FileUploader.hasPictureHelpFile = true;
                    }

                    var obj = {
                        isNew: true,
                        type: 4,
                        data: _.clone($scope.newField_FileUploader),
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 7;
                        obj.tablet_position.sizeY = 7;
                        obj.mobile_position.sizeY = 7;
                    }
                    else {
                        obj.desktop_position.sizeY = 9;
                        obj.tablet_position.sizeY = 9;
                        obj.mobile_position.sizeY = 9;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_FileUploader_Reset();

                    break;

                case 5:
                    if (pictureHelpFile != '') {
                        $scope.newField_Checkbox.pictureHelpFile = pictureHelpFile;
                        $scope.newField_Checkbox.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_Checkbox.hasPictureHelpFile = true;
                    }

                    var obj = {
                        isNew: true,
                        type: 5,
                        data: _.clone($scope.newField_Checkbox),
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 7;
                        obj.tablet_position.sizeY = 7;
                        obj.mobile_position.sizeY = 7;
                    }
                    else {
                        obj.desktop_position.sizeY = 9;
                        obj.tablet_position.sizeY = 9;
                        obj.mobile_position.sizeY = 9;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_Checkbox_Reset();

                    break;

                case 6:
                    if (pictureHelpFile != '') {
                        $scope.newField_WebUrl.pictureHelpFile = pictureHelpFile;
                        $scope.newField_WebUrl.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_WebUrl.hasPictureHelpFile = true;
                    }

                    var obj = {
                        isNew: true,
                        type: 6,
                        data: _.clone($scope.newField_WebUrl),
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 7;
                        obj.tablet_position.sizeY = 7;
                        obj.mobile_position.sizeY = 7;
                    }
                    else {
                        obj.desktop_position.sizeY = 9;
                        obj.tablet_position.sizeY = 9;
                        obj.mobile_position.sizeY = 9;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_WebUrl_Reset();

                    break;

                case 7:
                    if (pictureHelpFile != '') {
                        $scope.newField_DatePicker.pictureHelpFile = pictureHelpFile;
                        $scope.newField_DatePicker.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_DatePicker.hasPictureHelpFile = true;
                    }

                    var obj = {
                        isNew: true,
                        type: 7,
                        data: _.clone($scope.newField_DatePicker),
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 7;
                        obj.tablet_position.sizeY = 7;
                        obj.mobile_position.sizeY = 7;
                    }
                    else {
                        obj.desktop_position.sizeY = 9;
                        obj.tablet_position.sizeY = 9;
                        obj.mobile_position.sizeY = 9;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_DatePicker_Reset();

                    break;

                case 8:
                    if (pictureHelpFile != '') {
                        $scope.newField_DropDown.pictureHelpFile = pictureHelpFile;
                        $scope.newField_DropDown.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_DropDown.hasPictureHelpFile = true;
                    }

                    var data = _.clone($scope.newField_DropDown);
                    data.items = _.clone($scope.newField_DropDown.items);

                    var obj = {
                        isNew: true,
                        type: 8,
                        data: data,
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 7;
                        obj.tablet_position.sizeY = 7;
                        obj.mobile_position.sizeY = 7;
                    }
                    else {
                        obj.desktop_position.sizeY = 9;
                        obj.tablet_position.sizeY = 9;
                        obj.mobile_position.sizeY = 9;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);
                    $scope.newField_DropDown_Reset();
                    break;

                case 9:
                    if (pictureHelpFile != '') {
                        $scope.newField_MultipleChoice.pictureHelpFile = pictureHelpFile;
                        $scope.newField_MultipleChoice.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_MultipleChoice.hasPictureHelpFile = true;
                    }

                    var data = _.clone($scope.newField_MultipleChoice);
                    data.items = _.clone($scope.newField_MultipleChoice.items);

                    var obj = {
                        isNew: true,
                        type: 9,
                        data: data,
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 4 + 3 * obj.data.items.length;
                        obj.tablet_position.sizeY = 4 + 3 * obj.data.items.length;
                        obj.mobile_position.sizeY = 4 + 3 * obj.data.items.length;
                    }
                    else {
                        obj.desktop_position.sizeY = 6 + 3 * obj.data.items.length;
                        obj.tablet_position.sizeY = 6 + 3 * obj.data.items.length;
                        obj.mobile_position.sizeY = 6 + 3 * obj.data.items.length;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_MultipleChoice_Reset();

                    break;

                case 10:
                    if (pictureHelpFile != '') {
                        $scope.newField_CheckboxGroup.pictureHelpFile = pictureHelpFile;
                        $scope.newField_CheckboxGroup.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_CheckboxGroup.hasPictureHelpFile = true;
                    }

                    var data = _.clone($scope.newField_CheckboxGroup);
                    data.items = _.clone($scope.newField_CheckboxGroup.items);

                    var obj = {
                        isNew: true,
                        type: 10,
                        data: data,
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 4 + 3 * obj.data.items.length;
                        obj.tablet_position.sizeY = 4 + 3 * obj.data.items.length;
                        obj.mobile_position.sizeY = 4 + 3 * obj.data.items.length;
                    }
                    else {
                        obj.desktop_position.sizeY = 6 + 3 * obj.data.items.length;
                        obj.tablet_position.sizeY = 6 + 3 * obj.data.items.length;
                        obj.mobile_position.sizeY = 6 + 3 * obj.data.items.length;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_CheckboxGroup_Reset();

                    break;

                case 11:
                    if (pictureHelpFile != '') {
                        $scope.newField_FileUploader2.pictureHelpFile = pictureHelpFile;
                        $scope.newField_FileUploader2.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_FileUploader2.hasPictureHelpFile = true;
                    }

                    var obj = {
                        isNew: true,
                        type: 11,
                        data: _.clone($scope.newField_FileUploader2),
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 7;
                        obj.tablet_position.sizeY = 7;
                        obj.mobile_position.sizeY = 7;
                    }
                    else {
                        obj.desktop_position.sizeY = 9;
                        obj.tablet_position.sizeY = 9;
                        obj.mobile_position.sizeY = 9;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_FileUploader2_Reset();

                    break;

                case 12:
                    if (pictureHelpFile != '') {
                        $scope.newField_Label.pictureHelpFile = pictureHelpFile;
                        $scope.newField_Label.pictureHelpPath = '/Content/Upload/' + pictureHelpFile;
                        $scope.newField_Label.hasPictureHelpFile = true;
                    }

                    var obj = {
                        isNew: true,
                        type: 12,
                        data: _.clone($scope.newField_Label),
                        desktop_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 3
                        },
                        tablet_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 2
                        },
                        mobile_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        },
                        factor_position: {
                            //sizeX: 1,
                            //sizeY: 1,
                            //row: 1,
                            //col: 1
                        }
                    };

                    if (obj.data.description == null || obj.data.description == '') {
                        obj.desktop_position.sizeY = 7;
                        obj.tablet_position.sizeY = 7;
                        obj.mobile_position.sizeY = 7;
                    }
                    else {
                        obj.desktop_position.sizeY = 9;
                        obj.tablet_position.sizeY = 9;
                        obj.mobile_position.sizeY = 9;
                    }

                    obj.data.showCustomer = true;

                    $scope.form.fields.push(obj);

                    $scope.newField_Label_Reset();

                    break;
            }
        };

        $scope.editField = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/EditModalContent.html',
                controller: EditFieldCtrl,
                size: 'lg',
                resolve: {
                    fileFormats: function () {
                        return $scope.fileFormats;
                    },
                    fonts: function () {
                        return $scope.fonts;
                    },
                    field: function () {
                        var data = _.clone($scope.form.fields[index].data);
                        if ($scope.form.fields[index].type == 8 ||
                            $scope.form.fields[index].type == 9 ||
                            $scope.form.fields[index].type == 10) {
                            data.items = _.clone($scope.form.fields[index].data.items);
                        }
                        return data;
                    },
                    type: function () {
                        return $scope.form.fields[index].type;
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.form.fields[index].data = result;
            }, function () {
            });
        };

        var EditFieldCtrl = ['$scope', '$http', '$modalInstance', 'fileFormats', 'fonts', 'field', 'type', function ($scope, $http, $modalInstance, fileFormats, fonts, field, type) {

            $scope.fileFormats = fileFormats;
            $scope.fonts = fonts;
            $scope.field = field;
            $scope.type = type;
            $scope.newItem = {
                title: ''
            };
            $scope.PictureHelpFile = null;

            if (type == 4 || type == 11) {
                var fileTypes = _.clone($scope.field.fileTypes);
                $scope.field.fileTypes = [];
                
                for (i = 0; i < fileTypes.length; i++) {
                    $scope.field.fileTypes.push(_.find($scope.fileFormats, function (item) {
                        return item.Id == fileTypes[i].Id
                    }));
                }
            }

            $scope.addPictureHelpFile = function ($files) {
                var $file = $files[0];

                if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                    toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                    return;
                }

                if ($file.size > 150 * 1024) {
                    toaster.pop('error', "حداکثر حجم فایل 150 کیلو بایت می باشد");
                    return;
                }

                $scope.PictureHelpFile = $file;
            };

            $scope.removePictureHelpFile = function () {
                $scope.PictureHelpFile = null;
                $('#pictureHelpFileInput').val('');
                $('#pictureHelpFileInput').closest('.form-group').find('input[type=text]').val('');
                field.pictureHelpFile = '';
                field.pictureHelpPath = '';
                field.hasPictureHelpFile = false;
            };

            $scope.addItem = function () {
                if ($scope.newItem.title == null || $scope.newItem.title == '') return;
                $scope.field.items.push({
                    title: $scope.newItem.title
                });
                $scope.newItem.title = '';
            };

            $scope.moveUpItem = function (index) {
                if ($scope.field.items.length > 1 && index > 0) {
                    var temp = _.clone($scope.field.items[index]);
                    $scope.field.items[index] = _.clone($scope.field.items[index - 1]);
                    $scope.field.items[index - 1] = _.clone(temp);
                }

            };

            $scope.moveDownItem = function (index) {
                if ($scope.field.items.length > 1 && index < $scope.field.items.length - 1) {
                    var temp = _.clone($scope.field.items[index]);
                    $scope.field.items[index] = _.clone($scope.field.items[index + 1]);
                    $scope.field.items[index + 1] = _.clone(temp);
                }

            };

            $scope.removeItem = function (index) {
                $scope.field.items.splice(index, 1);
            };

            $scope.Item_PictureHelpFile = function (item, $event) {
                if (item.hasPictureHelpFile == false) {
                    $($event.currentTarget).closest('td').find('input[type=file]').click();
                }
                else {
                    ngDialog.open({
                        template: '<div style="font-family:yekan;font-size:14px;">' +
                              '<form>' +
                                '<div style="text-align:center;">' +
                                  '<button class="btn btn-lg btn-warning" ng-click="remove()" type="button">حذف فایل راهنما</button>' +
                                  '<button class="btn btn-lg btn-primary" ng-click="reselect()" type="button" style="margin-right:4px;">انتخاب مجدد فایل راهنما</button>' +
                                '</div>' +
                              '</form>' +
                            '</div>',
                        plain: true,
                        showClose: false,
                        className: 'ngdialog-theme-default ngdialog-theme1',
                        controller: ['$scope', 'ngDialog', function ($scope, ngDialog) {
                            $scope.paymentType = 1;

                            $scope.remove = function () {
                                $scope.closeThisDialog({
                                    response: 0,
                                    type: 0
                                });
                            };
                            $scope.reselect = function () {
                                $scope.closeThisDialog({
                                    response: 1,
                                    type: $scope.paymentType
                                });
                            };
                        }],
                        preCloseCallback: function (value) {
                            if (value.response == 0) {
                                $scope.Item_RemovePictureHelpFile(item);
                            }
                            else if (value.response == 1) {
                                $($event.currentTarget).closest('td').find('input[type=file]').click();
                            }
                            return true;
                        }
                    });
                }
            };

            $scope.Item_AddPictureHelpFile = function (item, $files) {
                var $file = $files[0];

                if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                    toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                    return;
                }

                if ($file.size > 150 * 1024) {
                    toaster.pop('error', "حداکثر حجم فایل 150 کیلو بایت می باشد");
                    return;
                }

                item.uploadingPictureHelpFile = true;

                $upload.upload({
                    url: baseUri + 'Form/UploadPicture',
                    file: $file
                }).success(function (data, status, headers, config) {
                    item.pictureHelpFile = data;
                    item.hasPictureHelpFile = true;
                    item.uploadingPictureHelpFile = false;
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                    }
                    item.uploadingPictureHelpFile = false;
                });
            };

            $scope.Item_RemovePictureHelpFile = function (item) {
                item.pictureHelpFile = '';
                item.hasPictureHelpFile = false;
            };

            $scope.save = function () {
                $scope.editFieldFromSubmited = true;
                if ($scope.editFieldFrom.$invalid) return;

                if ($scope.PictureHelpFile != null) {
                    $scope.editFieldLoading = true;
                    $upload.upload({
                        url: baseUri + 'Form/UploadPicture',
                        file: $scope.PictureHelpFile
                    }).success(function (data, status, headers, config) {
                        field.pictureHelpFile = data;
                        field.pictureHelpPath = '/Content/Upload/' + data;
                        field.hasPictureHelpFile = true;
                        $scope.editFieldLoading = false;
                        $modalInstance.close(field);
                    }).error(function (data, status, headers, config) {
                        if (status == 403) {
                            window.location = "/Account/Login";
                        }
                        else {
                            toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                        }
                        $scope.editFieldLoading = false;
                    });
                }
                else {
                    $modalInstance.close(field);
                }
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        $scope.removeField = function (index) {
            var templateDialog = 'removeDialog.html';
            if ($scope.form.fields[index].data.canDelete == false) {
                var templateDialog = 'removeDialog2.html';
            }

            ngDialog.open({
                template: templateDialog,
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

                    if ($scope.form.fields[index].isNew == null ||
                        $scope.form.fields[index].isNew == false) {
                        $scope.removedFields.push(_.clone($scope.form.fields[index].data.id));
                    }
                    $scope.form.fields.splice(index, 1);
                    return true;
                }
            });
        };

        $scope.showField = function (index) {
            $scope.form.fields[index].data.showCustomer = true;
        };

        $scope.hideField = function (index) {
            $scope.form.fields[index].data.showCustomer = false;
        };
        /*=-=-=-=-= End Manage Fields =-=-=-=-=*/

        /*=-=-=-=-= Start Manage Preview =-=-=-=-=*/
        $scope.gridsterOpts_desktop = {
            minRows: 2,
            maxRows: 2500,
            columns: 3,
            colWidth: 'auto',
            rowHeight: 10,
            margins: [5, 5],
            defaultSizeX: 1,
            defaultSizeY: 1,
            mobileBreakPoint: 600
        };

        $scope.gridsterOpts_tablet = {
            minRows: 2,
            maxRows: 2500,
            columns: 2,
            colWidth: 'auto',
            rowHeight: 10,
            margins: [5, 5],
            defaultSizeX: 1,
            defaultSizeY: 1,
            mobileBreakPoint: 600
        };

        $scope.gridsterOpts_mobile = {
            minRows: 2,
            maxRows: 2500,
            columns: 1,
            colWidth: 'auto',
            rowHeight: 10,
            margins: [5, 5],
            defaultSizeX: 1,
            defaultSizeY: 1,
            mobileBreakPoint: 250
        };

        $scope.gridsterOpts_factor = {
            minRows: 2,
            maxRows: 2500,
            columns: 1,
            colWidth: 'auto',
            rowHeight: 60,
            margins: [5, 5],
            defaultSizeX: 1,
            defaultSizeY: 1,
            mobileBreakPoint: 600
        };

        $scope.getFactorItems = function () {
            if ($scope.form == null || $scope.form.fields == null) return;

            var filter1 = _.filter($scope.form.fields, function (item) {
                return item.type == 0 || item.type == 1 || item.type == 2 ||
                    item.type == 5 || item.type == 7 || item.type == 8 ||
                    item.type == 9 || item.type == 10;
            });

            var filter2 = _.filter(filter1, function (item) {
                return item.data.showInFactor;
            });

            return filter2;
        };

        /*=-=-=-=-= End Manage Preview =-=-=-=-=*/

        /*=-=-=-=-= Start Connect To The Server =-=-=-=-=*/
        $scope.editForm = function () {
            if ($scope.formDetailsForm.$invalid || $scope.form.fields.length == 0) return;

            $scope.addLoading = true;
            $http.post(baseUri + 'Form/Edit',
            {
                form: {
                    Id: $scope.form.id,
                    Title: $scope.form.title,
                    Priority: $scope.form.priority,
                    SpecialCreativity: $scope.form.specialCreativity,
                    IsShow: $scope.form.isShow,
                    Description: $scope.form.description
                },
                groupId: $scope.form.group.Id,
                textBoxs: $scope.getFields(0, false),
                textBoxs_new: $scope.getFields(0, true),
                textAreas: $scope.getFields(1, false),
                textAreas_new: $scope.getFields(1, true),
                numerics: $scope.getFields(2, false),
                numerics_new: $scope.getFields(2, true),
                colorPickers: $scope.getFields(3, false),
                colorPickers_new: $scope.getFields(3, true),
                fileUploaders: $scope.getFields(4, false),
                fileUploaders_new: $scope.getFields(4, true),
                checkboxs: $scope.getFields(5, false),
                checkboxs_new: $scope.getFields(5, true),
                webUrls: $scope.getFields(6, false),
                webUrls_new: $scope.getFields(6, true),
                datePickers: $scope.getFields(7, false),
                datePickers_new: $scope.getFields(7, true),
                dropDowns: $scope.getFields(8, false),
                dropDowns_new: $scope.getFields(8, true),
                radioButtonGroups: $scope.getFields(9, false),
                radioButtonGroups_new: $scope.getFields(9, true),
                checkBoxGroups: $scope.getFields(10, false),
                checkBoxGroups_new: $scope.getFields(10, true),
                extendedFileUploaders: $scope.getFields(11, false),
                extendedFileUploaders_new: $scope.getFields(11, true),
                labels: $scope.getFields(12, false),
                labels_new: $scope.getFields(12, true),
                removedFields: $scope.removedFields
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

        $scope.getFields = function (type, isNew) {
            var filters = _.filter($scope.form.fields, function (item) {
                if (isNew == true) {
                    return item.type == type && item.isNew != null && item.isNew == true;
                }
                else {
                    return item.type == type && (item.isNew == 'undefined' || item.isNew == undefined || item.isNew == null || item.isNew == false);
                }
            });

            var fields = _.map(filters, function (item) {
                var obj = {};

                //Desktop Position
                obj.DesktopPosition = {
                    SizeX: item.desktop_position.sizeX,
                    SizeY: item.desktop_position.sizeY,
                    Row: item.desktop_position.row,
                    Column: item.desktop_position.col
                };

                //Tablet Position
                obj.TabletPosition = {
                    SizeX: item.tablet_position.sizeX,
                    SizeY: item.tablet_position.sizeY,
                    Row: item.tablet_position.row,
                    Column: item.tablet_position.col
                };

                //Mobile Position
                obj.MobilePosition = {
                    SizeY: item.mobile_position.sizeY,
                    Row: item.mobile_position.row
                };

                //Defualt Value
                if (item.isNew == null || item.isNew == false) {
                    obj.Id = item.data.id
                }
                obj.Title = item.data.title;
                obj.Description = item.data.description;
                obj.ShowCustomer = item.data.showCustomer;
                obj.ShowAdmin = true;
                obj.PictureHelpFile = item.data.pictureHelpFile;
                obj.Priority = item.data.priority;

                switch (item.type) {
                    //TextBox
                    case 0:
                        obj.Defualt = item.data.defualt;
                        obj.IsRequired = item.data.isRequired;
                        obj.CharacterLimits = item.data.characterLimits;
                        obj.MinCharacters = item.data.minCharacters;
                        obj.MaxCharacters = item.data.maxCharacters;
                        obj.ShowInFactor = item.data.showInFactor;
                        if (item.data.showInFactor != true) {
                            obj.FactorOrder = -1;
                        }
                        else {
                            obj.FactorOrder = item.factor_position.row;
                        }
                        break;

                    //TextArea
                    case 1:
                        obj.IsRequired = item.data.isRequired;
                        obj.CharacterLimits = item.data.characterLimits;
                        obj.MinCharacters = item.data.minCharacters;
                        obj.MaxCharacters = item.data.maxCharacters;
                        obj.ShowInFactor = item.data.showInFactor;
                        obj.Height = item.data.height;
                        if (item.data.showInFactor != true) {
                            obj.FactorOrder = -1;
                        }
                        else {
                            obj.FactorOrder = item.factor_position.row;
                        }
                        break;

                    //Numeric
                    case 2:
                        obj.IsInt = item.data.isInt;
                        obj.IsFloat = !item.data.isInt;
                        obj.Defualt = item.data.defualt;
                        obj.IsRequired = item.data.isRequired;
                        obj.Limits = item.data.limits;
                        obj.Min = item.data.min;
                        obj.Max = item.data.max;
                        obj.UseForPrice = item.data.useForPrice;
                        obj.ShowInFactor = item.data.showInFactor;
                        if (item.data.showInFactor != true) {
                            obj.FactorOrder = -1;
                        }
                        else {
                            obj.FactorOrder = item.factor_position.row;
                        }
                        break;

                    //Color Picker
                    case 3:
                        obj.IsRequired = item.data.isRequired;
                        break;

                    //File Uploader
                    case 4:
                        obj.IsRequired = item.data.isRequired;
                        obj.SizeLimits = item.data.sizeLimits;
                        obj.MinSize = item.data.minSize;
                        obj.MaxSize = item.data.maxSize;
                        obj.Formats = item.data.fileTypes;
                        if (item.data.showInFactor != true) {
                            obj.FactorOrder = -1;
                        }
                        else {
                            obj.FactorOrder = item.factor_position.row;
                        }
                        break;

                    //Checkbox
                    case 5:
                        obj.ShowInFactor = item.data.showInFactor;
                        obj.UseForPrice = item.data.useForPrice;
                        if (item.data.showInFactor != true) {
                            obj.FactorOrder = -1;
                        }
                        else {
                            obj.FactorOrder = item.factor_position.row;
                        }
                        break;

                    //Web Url
                    case 6:
                        obj.IsRequired = item.data.isRequired;
                        break;

                    //Date Picker
                    case 7:
                        obj.IsRequired = item.data.isRequired;
                        obj.Limits = item.data.limits;
                        obj.Min = item.data.min;
                        obj.Max = item.data.max;
                        obj.ShowInFactor = item.data.showInFactor;
                        if (item.data.showInFactor != true) {
                            obj.FactorOrder = -1;
                        }
                        else {
                            obj.FactorOrder = item.factor_position.row;
                        }
                        break;

                    //Drop Down
                    case 8:
                        obj.IsRequired = item.data.isRequired;
                        obj.ShowInFactor = item.data.showInFactor;
                        obj.UseForPrice = item.data.useForPrice;
                        if (item.data.showInFactor != true) {
                            obj.FactorOrder = -1;
                        }
                        else {
                            obj.FactorOrder = item.factor_position.row;
                        }

                        var order = 0;
                        obj.Items = _.map(item.data.items, function (data) {
                            return {
                                Id: data.id != null ? data.id : null,
                                Title: data.title,
                                Order: order++
                            };
                        })
                        break;

                    //Multiple Choice
                    case 9:
                        obj.IsRequired = item.data.isRequired;
                        obj.ShowInFactor = item.data.showInFactor;
                        obj.UseForPrice = item.data.useForPrice;
                        if (item.data.showInFactor != true) {
                            obj.FactorOrder = -1;
                        }
                        else {
                            obj.FactorOrder = item.factor_position.row;
                        }

                        var order = 0;
                        obj.Items = _.map(item.data.items, function (data) {
                            return {
                                Id: data.id != null ? data.id : null,
                                Title: data.title,
                                PictureHelpFile: data.pictureHelpFile,
                                Order: order++
                            };
                        })
                        break;

                    //Checkbox Group
                    case 10:
                        obj.ShowInFactor = item.data.showInFactor;
                        obj.UseForPrice = item.data.useForPrice;
                        if (item.data.showInFactor != true) {
                            obj.FactorOrder = -1;
                        }
                        else {
                            obj.FactorOrder = item.factor_position.row;
                        }

                        var order = 0;
                        obj.Items = _.map(item.data.items, function (data) {
                            return {
                                Id: data.id != null ? data.id : null,
                                Title: data.title,
                                PictureHelpFile: data.pictureHelpFile,
                                Order: order++
                            };
                        })
                        break;

                    //Extended File Uploader
                    case 11:
                        obj.IsRequired = item.data.isRequired;
                        obj.SizeLimits = item.data.sizeLimits;
                        obj.MinSize = item.data.minSize;
                        obj.MaxSize = item.data.maxSize;
                        obj.Formats = item.data.fileTypes;
                        if (item.data.showInFactor != true) {
                            obj.FactorOrder = -1;
                        }
                        else {
                            obj.FactorOrder = item.factor_position.row;
                        }
                        break;

                    //Label
                    case 12:
                        obj.FontFamily = item.data.font.Title;
                        obj.FontSize = item.data.size;
                        obj.Color = item.data.color;
                        obj.Underline = item.data.underline;
                        obj.Upline = item.data.upline;
                        break;
                }

                return obj;
            });

            return fields;
        };

        $scope.resetForm = function () {
            $scope.fetchForm();
            $scope.removedFields = [];
        };
        /*=-=-=-=-= End Connect To The Server =-=-=-=-=*/
    }]);