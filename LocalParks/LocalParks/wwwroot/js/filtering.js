$(document).ready(function () {

    $("#filter-form-parks").validate({
        rules: {
            searchTerm: {
                require_from_group: [1, ".filter-form-group"]
            },
            postcode: {
                require_from_group: [1, ".filter-form-group"]
            },
            sortBy: {
                require_from_group: [1, ".filter-form-group"]
            },
        },
        messages: {
            searchTerm: "",
            postcode: "",
            sortBy: ""
        }
    })

    $("#filter-form-sports-clubs").validate({
        rules: {
            searchTerm: {
                require_from_group: [1, ".filter-form-group"]
            },
            parkFilter: {
                require_from_group: [1, ".filter-form-group"]
            },
            sportType: {
                require_from_group: [1, ".filter-form-group"]
            },
            sortBy: {
                require_from_group: [1, ".filter-form-group"]
            },
        },
        messages: {
            searchTerm: "",
            parkFilter: "",
            sportType: "",
            sortBy: ""
        }
    })

    $("#filter-form-park-events").validate({
        rules: {
            searchTerm: {
                require_from_group: [1, ".filter-form-group"]
            },
            parkFilter: {
                require_from_group: [1, ".filter-form-group"]
            },
            date: {
                require_from_group: [1, ".filter-form-group"]
            },
            sortBy: {
                require_from_group: [1, ".filter-form-group"]
            },
        },
        messages: {
            searchTerm: "",
            parkFilter: "",
            date: "",
            sortBy: ""
        }
    })

    $("#filter-form-supervisors").validate({
        rules: {
            searchTerm: {
                require_from_group: [1, ".filter-form-group"]
            },
            parkFilter: {
                require_from_group: [1, ".filter-form-group"]
            },
            sortBy: {
                require_from_group: [1, ".filter-form-group"]
            },
        },
        messages: {
            searchTerm: "",
            parkFilter: "",
            sortBy: ""
        }
    })
});