var app = angular.module('licensegenerator', ['ui.bootstrap', 'angularFileUpload']);

function saveToDisk(fileURL, fileName) {
    // for non-IE
    if (!window.ActiveXObject) {
        var save = document.createElement('a');
        save.href = fileURL;
        save.target = '_blank';
        save.download = fileName || fileURL;
        var evt = document.createEvent('MouseEvents');
        evt.initMouseEvent('click', true, true, window, 1, 0, 0, 0, 0,
            false, false, false, false, 0, null);
        save.dispatchEvent(evt);
        (window.URL || window.webkitURL).revokeObjectURL(save.href);
    }

    // for IE
    else if (!!window.ActiveXObject && document.execCommand) {
        var _window = window.open(fileURL, "_blank");
        _window.document.close();
        _window.document.execCommand('SaveAs', true, fileName || fileURL)
        _window.close();
    }
}

$(function () {
    var loader = $(".loader2");
    var width = loader.parent().width();
    var height = loader.parent().height();
    load.
});

//angular.module('licensegenerator').directive('loadingContainer', function () {
//    return {
//        restrict: 'A',
//        scope: false,
//        link: function (scope, element, attrs) {
//            var loadingLayer = angular.element('<div class="loading"></div>');
//            element.append(loadingLayer);
//            element.addClass('loading-container');
//            scope.$watch(attrs.loadingContainer, function (value) {
//                loadingLayer.toggleClass('ng-hide', !value);
//            });
//        }
//    };
//});