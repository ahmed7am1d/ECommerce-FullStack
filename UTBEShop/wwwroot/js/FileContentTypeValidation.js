$.validator.addMethod('filecontent',
    function (value, element, params) {
        var fileContentType = params[1];

        var fileContentTypeFromFile = "";
        if (element && element.files && element.files.length > 0) {
            fileContentTypeFromFile = element.files[0].type;
        }

        if (!value || fileContentTypeFromFile && fileContentTypeFromFile != "" && fileContentTypeFromFile.toLowerCase().includes(fileContentType.toLowerCase())) {
            return true;
        }

        return false;
    });


$.validator.unobtrusive.adapters.add('filecontent', ['type'],
    function (options) {
        var element = $(options.form).find('#fileupload')[0];

        options.rules['filecontent'] = [element, options.params['type']];
        options.messages['filecontent'] = options.message;
    });