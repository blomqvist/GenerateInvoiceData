$(function () {
    var addProjectButton,
        deleteProjectButton,
        newRow = $("div[name=project-row-template]"),
        inputFields,
        previousChangeTime = new Date(),
        theForm = $("form#invoice-data-form"),
        currentRow = 0,
        autoCompleteData = {
            customers: [],
            projects: [],
            activities: []
        };

    const skipKeys = [8, 9, 16, 17, 18, 33, 34, 35, 36, 37, 39, 46];

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
            if (new Date() - previousChangeTime < 250) {
                return;
            }

            var data = theForm.serializeArray();

            $.post("/Home/Create", data)
                .done(function (response) {
                    $("textarea[name=invoice-data]").text(response);
                });

            previousChangeTime = new Date();
        });

        inputFields.keyup(function (event) {
            // Delete och backspace => ignore
            if (skipKeys.includes(event.keyCode)) {
                return;
            }

            var input = $(this);
            var parentDiv = input.closest("div.row");
            var inputsInRow = parentDiv.find("input[type=text]");

            var data = {};
            inputsInRow.each(function (key) {
                var element = $(inputsInRow[key]);
                data[element.attr("name")] = element.val();
            });
            $.post("/Home/AutoComplete", data)
                .done(function (response) {

                    autoCompleteData.customers = response.customers;
                    autoCompleteData.projects = response.projects;
                    autoCompleteData.activities = response.activities;

                    var inputName = input.attr("name").replace("[]", "");
                    var selectionStart = input.prop("selectionStart");
                    var inputVal = "";

                    if (inputName === "customers") {
                        inputVal = autoCompleteData.customers[0];
                    }

                    if (inputName === "projects") {
                        inputVal = autoCompleteData.projects[0];
                    }

                    if (inputName === "activities") {
                        inputVal = autoCompleteData.activities[0];
                    }

                    if (inputVal !== "" && inputVal !== undefined) {
                        input.val(inputVal);
                        input.prop("selectionStart", selectionStart);
                        input.prop("selectionEnd", inputVal.length);
                    }
                });
        });
    }

    init();
});