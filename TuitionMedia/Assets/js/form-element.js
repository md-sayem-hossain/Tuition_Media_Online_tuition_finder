$(document).ready(function () {
    window.asd = $('.SlectBox').SumoSelect({ csvDispCount: 3, selectAll: true, captionFormatAllSelected: "Yeah, OK, so everything." });

  window.testSelAlld = $('.SlectBox-grp').SumoSelect({ okCancelInMulti: true, selectAll: true, isClickAwayOk: true });

    window.sb = $('.SlectBox-grp-src').SumoSelect({ csvDispCount: 3, search: true, searchText: 'Enter here.', selectAll: true });
    
    $('.SlectBox').on('sumo:opened', function (o) {
      console.log("dropdown opened", o)
    });

  });