App.controller('EditPublicPriceController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 'ngDialog', '$stateParams',
    function ($scope, $http, baseUri, toaster, $modal, ngDialog, $stateParams) {
        $scope.editorOptions = {
            language: 'fa',
            uiColor: '#f2f2f2',
            resize_enabled: false,
            height: 290,
            enterMode: CKEDITOR.ENTER_BR,
            font_names: 'Arial;BNAZANB;BNazanin;IranianSerifWeb;NexaBold;Thoma;yekan;',// + config.font_names,
            toolbar: [
	            { name: 'document', groups: ['mode', 'document', 'doctools'], items: ['Source', '-', 'Save', 'NewPage', 'Preview', 'Print', '-', 'Templates'] },
	            { name: 'clipboard', groups: ['clipboard', 'undo'], items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
	            { name: 'editing', groups: ['find', 'selection', 'spellchecker'], items: ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt'] },
	            //{ name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'] },
	            '/',
	            { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
	            { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl'] },
	            { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
	            { name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'] },
	            '/',
	            { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
	            { name: 'colors', items: ['TextColor', 'BGColor'] },
	            { name: 'tools', items: ['Maximize', 'ShowBlocks'] },
	            { name: 'others', items: ['-'] }
            ],
            toolbarGroups: [
	            { name: 'document', groups: ['mode', 'document', 'doctools'] },
	            { name: 'clipboard', groups: ['clipboard', 'undo'] },
	            { name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
	            { name: 'forms' },
	            '/',
	            { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
	            { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
	            { name: 'links' },
	            { name: 'insert' },
	            '/',
	            { name: 'styles' },
	            { name: 'colors' },
	            { name: 'tools' },
	            { name: 'others' },
	            { name: 'about' }
            ],
            ImageBrowser: true,
            ImageBrowserURL: '/Vendors/ckeditor/finder/index.html',
            filebrowserImageBrowseUrl: '/Vendors/ckeditor/finder/index.html',
            filebrowserFlashBrowseUrl: '/Vendors/ckeditor/finder/index.html',
            filebrowserUploadUrl: '/Vendors/ckeditor/finder/connectors/ashx/filemanager.ashx',
            filebrowserImageUploadUrl: '/Vendors/ckeditor/finder/connectors/apsx/filemanager.ashx',
            filebrowserFlashUploadUrl: '/Vendors/ckeditor/finder/connectors/apsx/filemanager.ashx'
        };

        $scope.price = {};
        $scope.categories = [];

        $scope.fetchData = function () {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'PublicPrice/Find', {
                params: {
                    id: $stateParams.id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.price = data;
                $scope.fetchCategories();
                //$scope.fetchLoading = false;
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

        $scope.fetchCategories = function () {
            $scope.fetchCategoriesLoading = true;
            $http.get(baseUri + 'PublicPriceCategory/Get')
            .success(function (data, status, headers, config) {
                $scope.categories = data;
                var index = _.findIndex($scope.categories, function (item) {
                    return item.Id == $scope.price.Category.Id;
                });
                $scope.price.Category = $scope.categories[index];
                $scope.fetchCategoriesLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchCategoriesLoading = false;
            });
        };

        $scope.edit = function () {
            if ($scope.editPriceForm.$invalid &&
                $scope.price.Category == null) return;

            $scope.addLoading = true;
            $http.post(baseUri + 'PublicPrice/Edit',
            {
                model: {
                    Id: $scope.price.Id,
                    Title: $scope.price.Title,
                    Priority: $scope.price.Priority,
                    Description: $scope.price.Description
                },
                categoryId: $scope.price.Category.Id
            }).
            success(function (data, status, headers, config) {
                $scope.newPrice = {
                    Title: '',
                    Priority: 0,
                    Description: ''
                };
                $scope.newPrice.Category = $scope.categories[0];
                $scope.addLoading = false;
                toaster.pop('success', "پیام شما با موفقیت ویرایش شد");
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

        //init
        $scope.fetchData();
    }]);