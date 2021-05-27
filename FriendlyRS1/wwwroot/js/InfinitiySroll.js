
function InfinitiySroll(iTable, iAction, iParams, scroll, windowMode = false) {
   
    this.table = iTable;        // Reference to the table where data should be added
    this.action = iAction;      // Name of the conrtoller action
    this.params = iParams;      // Additional parameters to pass to the controller
    this.loading = false;       // true if asynchronous loading is in process
    this.AddTableLines = function (firstItem) {
        if (this.action != null) {
            this.loading = true;
            this.params.firstItem = firstItem;
            $.ajax({
                type: 'POST',
                url: self.action,
                data: self.params,
                dataType: "html"
            })
                .done(function (result) {
                    if (result) {
                        self.loading = false;
                        if (firstItem == 0) {
                            $("#" + self.table).html(result);
                            $("#" + self.table).hide().show('slow');
                        } else {
                            $("#" + self.table).append(result);

                        }
                    }
                })
                .fail(function (xhr, ajaxOptions, thrownError) {
                    console.log("Error in AddTableLines:", thrownError);
                })
                .always(function () {
                    // $("#footer").css("display", "none"); // hide loading info
                });
        }
    }

    // update state
    this.Update = function (action, params) {
        this.action = action;
        this.params = params;
        this.AddTableLines(0);
    }

    var self = this;
    if (windowMode == true) {
        window.onscroll = function (ev) {
            console.log(self.action);
            if ((window.innerHeight + window.pageYOffset) >= document.body.scrollHeight - 500) {
                // you're at the bottom of the page
                if (!self.loading) {
                    var itemCount = $('#' + self.table + ' > div').length;
                    //console.log("ADDDDD");
                    //console.log(itemCount);

                    self.AddTableLines(itemCount);
                }
            }
        };
    } else {
        $("#" + scroll).on('scroll', function (event) {
            let obj = document.getElementById(scroll);
            console.log("scrolling down");

            if (obj.scrollTop >= (obj.scrollHeight - obj.offsetHeight) - 1) {
                if (!self.loading) {
                    var itemCount = $('#' + self.table + ' > div').length;
                    //console.log("Adding new lines " + itemCount);

                    self.AddTableLines(itemCount);
                }
            }
        });
    }

    this.AddTableLines(0);
}
