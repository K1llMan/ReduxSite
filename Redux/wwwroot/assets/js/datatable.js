/*-----------------------------------------------------------------------------
                             (jQuery 3.3.1)
-----------------------------------------------------------------------------*/
(function ($) {
    var datatable;
    var opt = {
        'tableHeader': 'Testiiiiiing',
        'fields': {
            'col1': {
                'header': 'Column 1',
                'tooltip': 'Column 1',
                'hidden': false,
                'editable': false
            },
            'col2': {
                'header': 'Column 2',
                'tooltip': 'Column 2',
                'hidden': false,
                'editable': false
            },
            'col3': {
                'header': 'Column 3',
                'tooltip': 'Column 3',
                'hidden': false,
                'editable': true,
                'size': 10
            },
            'col4': {
                'header': 'Column 4',
                'tooltip': 'Column 4',
                'hidden': false,
                'editable': true,
                'size': 1000,
                'afterEdit': function(row) {
                    alert(row);
                }
            }
        },
        'data': {
            'total': 100,
            'page': 1,
            'pageSize': 10,
            'pageCount': 1,
            'rows': [],            
        },
        'hideHeader': false,
        'hideFooter': false,
        'hideSelection': false,
        'getData': function(page, pageSize, sort, sortDir, updateData) {
            var response = {
                'total': 100,
                'page': page,
                'pageSize': pageSize,
                'pageCount': 10,
                'rows': [
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
        },
        'beforeDelete': function(rows) {
            
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
        table.setHeaders = function() {
            var head = this.find('thead tr');
            head.append(addHeaderCheckBox());
            $.each(Object.keys(opt.fields), function (i, el) {
                var cell = $('<th></th>');
                cell.attr('id', el);
                cell.html(opt.fields[el].header);

                head.append(cell);
            });
        }

        table.setRows = function() {
            var body = this.find('tbody');
            body.html('');
            for (var i = 0; i < opt.data.pageSize; i++) {
                var tableRow = $('<tr></tr>');
                tableRow.attr('num', i);
                tableRow.append(addCheckBox('check_' + i));

                $.each(Object.keys(opt.fields), function (i, key) {
                    var cell = $('<td></td>');
                    cell.attr('id', key);
                    cell.html();

                    var field = opt.fields[key];

                    if (field && field.editable) {
                        cell.toggleClass('editable');

                        cell.click(function() {
                            var dialog = datatable.editDialog;
                            // Set position and header
                            dialog.find('label').html(field.header);
                            dialog.css(cell.offset());

                            // Init input area
                            var area = dialog.find('#editArea');
                            area.val(cell.html());
                            area.trigger('autoresize');

                            // Init character counter, size must be always set
                            area.attr('length', field.size);
                            area.characterCounter();

                            dialog.show();
                            area.focus();

                            // Set save handler
                            var btnSave = dialog.find('#btnSave');
                            btnSave.off('click');
                            btnSave.click(function () {
                                var value = area.val();
                                if (value.length > field.size) {
                                    Materialize.toast('Value is too long', 3000);
                                    return;
                                }

                                var rowNum = parseInt(cell.closest('tr').attr('num'), 10);
                                var rowData = opt.data.rows[rowNum];
                                rowData[key] = area.val();
                                cell.html(rowData[key]);

                                if (field.afterEdit)
                                    field.afterEdit(rowData);

                                dialog.hide();
                            });
                        });
                    }

                    tableRow.append(cell);
                });

                body.append(tableRow);
            }

            datatable.table.hideColumns();
            datatable.table.hideSelection();
        }

        table.updateData = function (data) {
            opt.data = data;

            // Update counters
            datatable.footer.counter.update();

            datatable.footer.find("#page-left").toggleClass('disabled', opt.data.page == 1);
            datatable.footer.find("#page-right").toggleClass('disabled', opt.data.page == opt.data.pageCount);

            // Fill rows set
            table.setRows();

            // Fill rows
            var body = table.find('tbody');
            $.each(opt.data.rows, function (i, row) {
                var tableRow = body.find('tr:nth-child(' + (i + 1) + ')');
                if (tableRow.length == 0)
                    return;

                tableRow.css({ 'borderBottom': '' });
                tableRow.show();

                $.each(Object.keys(row), function (i, key) {
                    var cell = tableRow.find('#' + key);
                    cell.html(row[key]);
                });
            });

            //body.find('tr:nth-child(' + (opt.data.rows.length) + ')').css({ 'borderBottom': 'none' });
            body.find('tr:nth-child(n+' + (opt.data.rows.length + 1) + ')').hide();
        }

        table.getSelected = function() {
            return this.find('tbody tr:visible').has('input:checked');
        }

        table.removeSelected = function() {
            var rowsData = table.getSelected().map(function (i, row) {
                return opt.data.rows[parseInt($(row).attr('num'), 10)];
            });

            datatable.table.find('thead input').prop('checked', false);
            datatable.table.find('thead input').prop('indeterminate', false);

            if (rowsData.length == 0)
                return;

            if (opt.beforeDelete)
                opt.beforeDelete(rowsData);

            Materialize.toast(rowsData.length + ' rows removed.', 3000);
            datatable.table.getData();
        }

        table.getData = function() {
            if (!opt.getData)
                return;

            opt.getData(opt.data.page, opt.data.pageSize, null, 'asc', table.updateData);
        }

        table.hideColumns = function() {
            // Show all columns
            this.find('th:not(.select),td:not(.select)').show();

            // Hide selected
            $.each(Object.keys(opt.fields), function (i, key) {
                if (!opt.fields[key].hidden)
                    return;

                var cols = datatable.find('table #' + key);
                cols.hide();
            });            
        }

        table.hideSelection = function() {
            var column = this.find('table .select');
            if (opt.hideSelection)
                column.hide();
            else
                column.show();            
        }

        datatable.append(table);
        datatable.table = table;        
    }

    function getFooter() {
        var footer = $('<div class="footer"><label style="margin-left: auto;">Rows per page:</label></div>');

        var pageSizeControl = $('<select class="browser-default">' +
                '<option value="10" selected>10</option>' + 
                '<option value="25">25</option>' + 
                '<option value="50">50</option>' + 
            '</select>');
        footer.append(pageSizeControl);
        footer.find('select').change(function() {
            opt.data.pageSize = parseInt(this.value, 10);
            datatable.table.getData();
        });

        var counter = $('<label></label>');
        counter.update = function () {
            var start = (opt.data.page - 1) * opt.data.pageSize + 1;
            var end = start + opt.data.rows.length - 1;
            counter.html(start + '-' + end + ' of ' + opt.data.total);
        };

        footer.append(counter);
        footer.counter = counter;

        var pageControl = $('<div class="pagination"></div>');
        var left = $('<a id="page-left" class="disabled"><i class="material-icons waves-effect">chevron_left</i></a>');
        left.click(function () {
            if (opt.data.page > 1) {
                opt.data.page--;
                datatable.table.getData();
            }
        });

        var right = $('<a id="page-right" class="disabled"><i class="material-icons waves-effect">chevron_rights</i></a>');
        right.click(function () {
            if (opt.data.page < opt.data.pageCount) {
                opt.data.page++;
                datatable.table.getData();
            }
        });

        pageControl.append(left, right);
        footer.append(pageControl);

        footer.hide = function(hide) {
            this.toggleClass('hide', hide);
        }

        datatable.append(footer);
        datatable.footer = footer;
    }

    function getEditDialog() {
        var dialog = $('<div class="z-depth-1 row" id="editDialog"></div>');

        var btnSave = $('<a id="btnSave" class="right waves-effect teal-text btn-flat">Save</a>');
        var btnCancel = $('<a class="right waves-effect teal-text btn-flat">Cancel</a>');
        var btnClear = $('<a class="right waves-effect teal-text btn-flat">Clear</a>');

        btnCancel.click(function () {
            dialog.hide();
        });

        btnClear.click(function () {
            dialog.find('#editArea').val('');
            dialog.find('#editArea').blur();
        });

        var textArea = $('<div class="input-field col s12">' +
                '<textarea id="editArea" class="materialize-textarea" length="120"></textarea>' + 
                '<label for="editArea"></label>' + 
            '</div>');

        dialog.append(textArea, btnSave, btnCancel, btnClear);

        datatable.append(dialog);
        datatable.editDialog = dialog;
        dialog.hide();
    }

    function init() {
        // Clear html struct
        datatable.html('');
        getHeader();
        getTable();
        getFooter();
        getEditDialog();
    }

    $.fn.datatable = function (options) {
        datatable = this;
        init();

        // Header
        datatable.header.setCaption(opt.tableHeader);
        datatable.header.hide(opt.hideHeader);

        datatable.table.setHeaders();
        datatable.table.hideColumns();

        datatable.table.hideSelection();

        datatable.footer.hide(opt.hideFooter);

        datatable.table.getData();

        return datatable;
    };
})(jQuery);

var dt;
$(function() {
    dt = $('.datatable').datatable();
});