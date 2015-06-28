App.controller('NewSendMessageController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', 'ngDialog',
    function ($scope, $http, baseUri, toaster, $modal, ngDialog) {
        $scope.froalaOptions = {
            buttons: ["bold", "italic", "underline", "strikeThrough", "fontFamily",
                "fontSize", "color", "formatBlock", "align", "insertOrderedList",
                "insertUnorderedList", "outdent", "indent", "selectAll", "createLink",
                "insertVideo", "undo", "redo", "inserthorizontalrule"],
            inlineMode: false,
            inverseSkin: true,
            allowedImageTypes: ["jpeg", "jpg", "png"],
            height: 300,
            language: "fa",
            direction: "rtl",
            fontList: ["Tahoma, Geneva", "Arial, Helvetica", "Impact, Charcoal"],
            spellcheck: true,
            plainPaste: true,
            borderColor: '#00008b',
            enableScript: false
        };

        $scope.message = {
            Title: '',
            Text: ''
        };

        $scope.send = function () {
            if ($scope.sendMessageForm.$invalid) return;

            $scope.sendLoading = true;
            $http.post(baseUri + 'SendMessage/New',
            {
                message: $scope.message
            }).
            success(function (data, status, headers, config) {
                $scope.message = {
                    Title: '',
                    Text: ''
                };
                $scope.sendLoading = false;
                toaster.pop('success', "پیام شما با موفقیت ارسال شد");
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
    }]);