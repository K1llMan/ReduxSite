/*-----------------------------------------------------------------------------
                             (jQuery 3.3.1)
-----------------------------------------------------------------------------*/
(function ($) {
    var datatable;
    var opt = {
        'tableHeader': 'Testiiiiiing',
        'headers': { 'col1': "Column 1", 'col2': "Column 2", 'col3': "Column 3" },
        'data': [
            { 'col1': 'data1', 'col2': 'data2', 'col3': 'data3' },
            { 'col1': 'data11', 'col2': 'data21', 'col3': 'data31' },
            { 'col1': 'data41', 'col2': 'data41', 'col3': 'data41' },
            { 'col1': 'data12', 'col2': 'data22', 'col3': 'data32' }
        ],
        'page': 1,
        'pageSize': 10,
        'pageCount': 1,
        'hideHeader': false,
        'hideFooter': true,
        'hideSelection': false,
        'hidden': [ ]
    };

    function addHeaderCheckBox() {
        var checkBox = $('<th class="select"><label><input type="checkbox"/><span></span></label></th>');
        checkBox.find('label input').attr('id', 'headerCheck');
        return checkBox;
    }

    function addCheckBox(id) {
        var checkBox = $('<td class="select"><label><input type="checkbox"/><span></span></label></td>');
        checkBox.find('label input').attr('id', id);
        return checkBox;
    }

    function getHeader() {
        var header = $('<div class="header"></div >');
        header.setCaption = function (caption) {
            this.html(caption);
        };
        header.hide = function (hide) {
            this.toggleClass('hide', hide);
        }

        datatable.append(header);
        datatable.header = header;
    }

    function getTable() {
        var table = $('<div class="data"><table><thead><tr></tr></thead><tbody></tbody></table></div>');
        table.setHeaders = function(headers) {
            var head = this.find('thead tr');
            head.append(addHeaderCheckBox());
            $.each(Object.keys(headers), function (i, el) {
                var cell = $('<th></th>');
                cell.attr('id', el);
                cell.html(headers[el]);

                head.append(cell);
            });

            table.headers = Object.keys(headers);
        }

        table.setRows = function(rowsCount) {
            var body = this.find('tbody');
            body.html('');
            for (var i = 0; i < rowsCount; i++) {
                var tableRow = $('<tr></tr>');
                tableRow.append(addCheckBox('check_' + i));

                $.each(this.headers, function (i, el) {
                    var cell = $('<td></td>');
                    cell.attr('id', el);
                    cell.html();

                    tableRow.append(cell);
                });

                body.append(tableRow);
            }
        }

        table.updateData = function(data) {
            var body = this.find('tbody');
            $.each(data, function (i, row) {
                var tableRow = body.find('tr:nth-child(' + (i + 1) + ')');
                if (tableRow.length == 0)
                    return;

                tableRow.css({ 'borderBottom': '' });
                tableRow.show();

                $.each(Object.keys(row), function (i, key) {
                    tableRow.find('#' + key).html(row[key]);
                });
            });

            body.find('tr:nth-child(' + (data.length) + ')').css({ 'borderBottom': 'none' });
            body.find('tr:nth-child(n+' + (data.length + 1) + ')').hide();            
        }

        table.hideColumns = function(columns) {
            // Show all columns
            this.find('th:not(.select),td:not(.select)').show();

            // Hide selected
            $.each(columns, function (i, name) {
                var cols = datatable.find('table #' + name);
                cols.hide();
            });            
        }

        table.hideSelection = function(hide) {
            var column = this.find('table .select');
            if (hide)
                column.hide();
            else
                column.show();            
        }

        datatable.append(table);
        datatable.table = table;        
    }

    function getFooter() {
        var footer = $('<div class="footer"></div>');
        footer.hide = function(hide) {
            this.toggleClass('hide', hide);
        }

        datatable.append(footer);
        datatable.footer = footer;
    }

    function init() {
        // Clear html struct
        datatable.html('');
        getHeader();
        getTable();
        getFooter();
    }

    $.fn.datatable = function (options) {
        datatable = this;
        init();

        // Header
        datatable.header.setCaption(opt.tableHeader);
        datatable.header.hide(opt.hideHeader);

        datatable.table.setHeaders(opt.headers);
        datatable.table.setRows(opt.pageSize);
        datatable.table.updateData(opt.data);

        datatable.table.hideColumns(opt.hidden);
        datatable.table.hideSelection(opt.hideSelection);


        datatable.footer.hide(opt.hideFooter);

        return datatable;
    };
})(jQuery);

$(function() {
    $('.datatable').datatable();
});