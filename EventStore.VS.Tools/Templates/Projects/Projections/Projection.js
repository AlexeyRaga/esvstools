fromAll()
.when({
    $init: function () {
        return { count: 0, }; // initial state
    },

    event_type_1: function (s, e) {
        s.count += 1;
        return s;
    },
});