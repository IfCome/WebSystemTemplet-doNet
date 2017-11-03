$(function () {

    // 侧边菜单开关
    autoLeftNav();
    $(window).resize(function () {
        autoLeftNav();
        //console.log($(window).width())
    });

    // 全屏
    var $fullText = $('.admin-fullText');
    $('#admin-fullscreen').on('click', function () {
        $.AMUI.fullscreen.toggle();
    });
    $(document).on($.AMUI.fullscreen.raw.fullscreenchange, function () {
        $fullText.text($.AMUI.fullscreen.isFullscreen ? '退出全屏' : '开启全屏');
    });

    // 风格切换
    $('.tpl-skiner-toggle').on('click', function () {
        $('.tpl-skiner').toggleClass('active');
    })
    $('.tpl-skiner-content-bar').find('span').on('click', function () {
        $('body').attr('class', $(this).attr('data-color'))
        saveSelectColor.Color = $(this).attr('data-color');
        // 保存选择项
        storageSave(saveSelectColor);

    })

    // 侧边菜单
    $('.sidebar-nav-sub-title').on('click', function () {
        $(this).siblings('.sidebar-nav-sub').slideToggle(80)
            .end()
            .find('.sidebar-nav-sub-ico').toggleClass('sidebar-nav-sub-ico-rotate');
    })

    // 判断用户是否已有自己选择的模板风格
    if (storageLoad('SelcetColor')) {
        $('body').attr('class', storageLoad('SelcetColor').Color)
    } else {
        storageSave(saveSelectColor);
        $('body').attr('class', 'theme-white')
    }

    // 设置当前选中的菜单
    var selectMenuId = $('#admin-current-menu-id').val();
    var selectMenu = $('#' + selectMenuId);
    if (selectMenu.length > 0) {
        if (selectMenu.closest('ul').hasClass('sidebar-nav-sub')) {
            selectMenu.addClass('sub-active').closest('ul').css('display', 'block').prev().addClass('active');
        } else {
            selectMenu.addClass('active')
        }
    }

})

// 侧边菜单开关
function autoLeftNav() {
    $('.tpl-header-switch-button').on('click', function () {
        if ($('.left-sidebar').is('.active')) {
            if ($(window).width() > 1024) {
                $('.tpl-content-wrapper').removeClass('active');
            }
            $('.left-sidebar').removeClass('active');
        } else {
            $('.left-sidebar').addClass('active');
            if ($(window).width() > 1024) {
                $('.tpl-content-wrapper').addClass('active');
            }
        }
    })

    if ($(window).width() < 1024) {
        $('.left-sidebar').addClass('active');
    } else {
        $('.left-sidebar').removeClass('active');
    }
}


var saveSelectColor = {
    'Name': 'SelcetColor',
    'Color': 'theme-white'
}

// 本地缓存
function storageSave(objectData) {
    localStorage.setItem(objectData.Name, JSON.stringify(objectData));
}

function storageLoad(objectName) {
    if (localStorage.getItem(objectName)) {
        return JSON.parse(localStorage.getItem(objectName))
    } else {
        return false
    }
}

// 加载loading
function openLoading(msg) {
    if ($('#my-modal-loading').length > 0) {
        $('#my-modal-loading .am-modal-hd').text(msg || '正在载入...');
        $('#my-modal-loading').modal('open');
        $('.am-dimmer').css('display', 'none');
    }
}
function closeLoading() {
    if ($('#my-modal-loading').length > 0) {
        $('#my-modal-loading').modal('close');
    }
}

window.onbeforeunload = function() {
    openLoading('正在跳转...');
}
// ajax全局处理
$.ajaxSetup({
    type: "POST",
    error: function (jqXHR, textStatus, errorThrown) {
        switch (jqXHR.status) {
            case (500):
                alert("服务器系统内部错误");
                break;
            case (401):
                window.location.href = "/admin/home/indexpage.html";
                break;
            case (403):
                alert("无权限执行此操作");
                break;
            case (404):
                alert("请求路径错误");
                break;
            case (408):
                alert("请求超时");
                break;
            default:
                alert("未知错误");
        }
    }
});
