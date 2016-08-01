var OneHUDDataProtocol = function () {
    'use strict';

    return {
        SessionType: {
            Invalid : 0,
            OfflinePractice : 1,
            Practice : 2,
            Qualifying : 3,
            Race : 4
        },

        SessionName: [
            'Invalid',
            'Offline Practice',
            'Practice',
            'Qualifying',
            'Race'
        ]
    }
}();