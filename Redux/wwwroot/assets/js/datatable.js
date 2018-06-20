/*-----------------------------------------------------------------------------
                             (jQuery 3.3.1)
-----------------------------------------------------------------------------*/
(function ($) {
    var datatable;
    var opt = {
        'tableHeader': 'Testiiiiiing',
        'headers': {
            'col1': "Column 1", 'col2': "Column 2", 'col3': "Column 3", 'col4': 'Column 4', 'col5': 'Column 5', 'col6': 'Column 6',
            'col7': 'Column 7', 'col8': 'Column 8', 'col9': 'Column 9', 'col10': 'Column 10'
        },
        'data': [],
        'total': 100,
        'page': 1,
        'pageSize': 10,
        'pageCount': 1,
        'hideHeader': false,
        'hideFooter': false,
        'hideSelection': false,
        'hidden': [],
        'getData': function(page, pageSize, sort, sortDir, updateData) {
            var response = {
                'total': 100,
                'page': page,
                'pageSize': pageSize,
                'data': [
                    { 'col1': 'data41', 'col2': 'data41', 'col3': 'data41', 'col4': 'data51', 'col5': 'ololo', 'col6': '434525', 'col7': 'Column 7', 'col8': 'Column 8', 'col9': 'Column 9', 'col10': 'Column 10 dfgsdg sdg sdg  sdgsdgwegsd segsd gsdg serg sdgwsegsdfg sergsdg segsdfgdLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.' },
                    { 'col1': 'data1', 'col2': 'data2', 'col3': 'data3' },
                    { 'col1': 'data41', 'col2': 'data41', 'col3': 'data41' },
                    { 'col1': 'data11', 'col2': 'data21', 'col3': 'data31' },
                    { 'col1': 'data41', 'col2': 'data41', 'col3': 'data41' },
                    { 'col1': 'data12', 'col2': 'data22', 'col3': 'data32' },
                    { 'col1': 'data41', 'col2': 'data41', 'col3': 'data41' }
                ]
            }

            updateData(response);
        }
    };

    function addHeaderCheckBox() {
        var checkBox = $('<th class="select"><label><input type="checkbox"/><span></span></label></th>');
        checkBox.find('label input').attr('id', 'headerCheck');
        checkBox.find('label input').change(function () {
            var isChecked = $(this).is(":checked");

            datatable.table.find('tbody input:visible')
                .prop("checked", isChecked);
        });

        return checkBox;
    }

    function addCheckBox(id) {
        var checkBox = $('<td class="select"><label><input type="checkbox"/><span></span></label></td>');
        checkBox.find('label input').attr('id', id);

        checkBox.change(function () {
            var headerCheck = datatable.table.find('thead input');

            var selectedCount = 0;
            var count = 0;
            datatable.table.find('tbody input:visible')
                .map(function (i, check) {
                    count++;
                    if ($(check).is(":checked"))
                        selectedCount++;
                });

            headerCheck.prop("indeterminate", false);
            if (selectedCount == count)
                headerCheck.prop("checked", true);
            else if (selectedCount == 0)
                headerCheck.prop("checked", false);
            else
                headerCheck.prop("indeterminate", true);
        });

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

        table.setRows = function() {
            var body = this.find('tbody');
            body.html('');
            for (var i = 0; i < opt.pageSize; i++) {
                var tableRow = $('<tr></tr>');
                tableRow.attr('num', i);
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

        table.updateData = function (data) {
            opt.data = data.data;

            opt.total = data.total;
            opt.page = data.page;
            opt.pageSize = data.pageSize;
            opt.pageCount = opt.total / opt.pageSize;

            table.setRows();

            var body = table.find('tbody');
            $.each(opt.data, function (i, row) {
                var tableRow = body.find('tr:nth-child(' + (i + 1) + ')');
                if (tableRow.length == 0)
                    return;

                tableRow.css({ 'borderBottom': '' });
                tableRow.show();

                $.each(Object.keys(row), function (i, key) {
                    tableRow.find('#' + key).html(row[key]);
                });
            });

            body.find('tr:nth-child(' + (opt.data.length) + ')').css({ 'borderBottom': 'none' });
            body.find('tr:nth-child(n+' + (opt.data.length + 1) + ')').hide();            
        }

        table.getSelected = function() {
            return this.find('tbody tr:visible').has('input:checked');
        }

        table.getData = function() {
            if (!opt.getData)
                return;

            opt.getData(opt.page, opt.pageSize, null, 'asc', table.updateData);
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
        var footer = $('<div class="footer"><label style="margin-left: auto;">Rows per page</label></div>');

        var pageSizeControl = $('<select class="browser-default">' +
                '<option value="10" selected>10</option>' + 
                '<option value="25">25</option>' + 
                '<option value="50">50</option>' + 
            '</select>');
        footer.append(pageSizeControl);
        footer.find('select').change(function() {
            opt.pageSize = parseInt(this.value, 10);
            datatable.table.getData();
        });

        var pageControl = $('<div class="pagination"><a><i class="material-icons">chevron_left</i></a><a><i class="material-icons">chevron_rights</i></a></div>');
        footer.append(pageControl);


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

        datatable.addClass('z-depth-1');
    }

    $.fn.datatable = function (options) {
        datatable = this;
        init();

        // Header
        datatable.header.setCaption(opt.tableHeader);
        datatable.header.hide(opt.hideHeader);

        datatable.table.setHeaders(opt.headers);
        datatable.table.getData();

        datatable.table.hideColumns(opt.hidden);
        datatable.table.hideSelection(opt.hideSelection);

        datatable.footer.hide(opt.hideFooter);

        return datatable;
    };
})(jQuery);

$(function() {
    $('.datatable').datatable();
});