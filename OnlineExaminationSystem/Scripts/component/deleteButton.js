(function ($) {
    $.fn.deleteButton = function () {
        $(this).each(function () {
            var $this = $(this);
            var confim = document.createElement("div");
            var confirmOk = document.createElement('a');
            confirmOk.innerHTML = "<i class='fa fa-check'></i>";
            confirmOk.href = $this.data('delete-url');
            var question = document.createElement("div");
            question.innerHTML =  "Are you sure?";
            var confirmCancel = document.createElement('a');
            confirmCancel.innerHTML = "<i class='fa fa-close'></i>";
            confirmCancel.href = "#";
            confim.appendChild(question);
            confim.appendChild(confirmOk);
            confim.appendChild(confirmCancel);
            var $confirm = $(confim).hide();
            $this.after($confirm);
            $this.click(function () {
                $this.fadeOut(500);
                $confirm.fadeIn(200);
                return false;
            });

            $(confirmCancel).click(function () {
                $confirm.fadeOut(500);
                $this.fadeIn(200);
                return false;
            });
        })
    }
})(jQuery)