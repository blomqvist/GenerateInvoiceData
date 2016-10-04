// Write your Javascript code.
$(function () {
    var addProjectButton,
        deleteProjectButton,
        newRow = $("div[name=project-row-template]"),
        inputFields,
        previousChangeTime = new Date(),
        theForm = $("form#invoice-data-form"),
        currentRow = 0;

    function init() {
        addProjectButton = $("button[name=add-project]");
        deleteProjectButton = $("button[name=delete-project]");
        inputFields = $("input");

        addProjectButton.click(function () {
            newRow.attr("data-project-row", ++currentRow);
            $(this).parent().find("button").remove();
            $("fieldset[name=project-rows]").append(newRow.clone());

            init();
        });

        deleteProjectButton.click(function () {
            currentRow--;
            $(this).parent().parent().remove();

            var addButton = $("button[name=add-project]");
            var deleteButton = $("button[name=delete-project]");

            var buttonDiv = $("div[data-project-row=" + currentRow + "] > div[name=button-div]");

            buttonDiv.append(addButton.clone());

            if (currentRow > 0) {
                buttonDiv.append(deleteButton.clone());
            }

            init();
        });

        inputFields.change(function () {
            if (new Date() - previousChangeTime < 1000) {
                return;
            }

            var data = theForm.serializeArray();

            $.post("/Home/Create", data)
                .done(function(response) {
                    $("textarea[name=invoice-data]").text(response);
                });

            previousChangeTime = new Date();
        });
    }

    init();
});