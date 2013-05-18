fromAll()
.when({
    $init: function () {
        return { count: 0, }; // initial state
    },

    some_event_type: function (s, e) {
        s.count += 1;
        return s;
    },
});