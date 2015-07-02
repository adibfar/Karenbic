App.controller('PrePrintOrderController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 'ngDialog',
    function ($scope, $http, baseUri, toaster, $modal, ngDialog) {

        $scope.text = '';

        $scope.froalaOptions = {
            buttons: ["bold", "italic", "underline", "strikeThrough", "fontFamily",
                    "fontSize", "color", "formatBlock", "align", "insertOrderedList",
                    "insertUnorderedList", "outdent", "indent", "selectAll", "createLink",
                    "insertImage", "insertVideo", "undo", "redo", "html", "inserthorizontalrule"],
            inlineMode: false,
            inverseSkin: true,
            allowedImageTypes: ["jpeg", "jpg", "png"],
            height: 500,
            language: "fa",
            direction: "rtl",
            fontList: ["Tahoma, Geneva", "Arial, Helvetica", "Impact, Charcoal"],
            spellcheck: true,
            plainPaste: true,
            imageButtons: ["removeImage", "replaceImage", "linkImage"],
            borderColor: '#00008b',
            imageUploadURL: baseUri + 'Froala/UploadImage',
            enableScript: false
        };

        $scope.fetchText = function () {
            $scope.fetchLoading = true;
            $http.get(baseUri + 'PrintOrder/PreOrderText')
            .success(function (data, status, headers, config) {
                $scope.text = data;
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

        $scope.save = function () {
            $scope.sendLoading = true;
            $http.post(baseUri + 'PrintOrder/PreOrderText',
            {
                text: $scope.text
            }).
            success(function (data, status, headers, config) {
                $scope.sendLoading = false;
                toaster.pop('success', "متن با موفقیت ذخیره گردید");
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.sendLoading = false;
            });
        };

        //init 
        $scope.fetchText();
    }]);