// Jacob Janak 2017

// Customizable variables
var jobsPerPage = $("#jobs-per-page :selected").val(); // Amount of jobs to be displayed at once
var maxPagesDisplayed = 5; // Limits the number of pagination links displayed at once. Must be an odd number.

// Global variables
var rows = document.getElementsByTagName("tr"); // Live node-list of all rows. Note: this includes table header
var rowsFiltered = document.querySelectorAll("tr:not(.filtered)"); // Gets only rows matching search criteria. Note: not a live list
var pageLinks = document.getElementsByClassName("paginationLink");
var totalPages = Math.ceil((rowsFiltered.length - 1) / jobsPerPage); // How many pages are needed to fit all jobs

// Script begins
hideExtraRows(1); // Initialize the pagination to page 1
$("#job-listings-table").tablesorter(); // Enable tableSorter.js

// Event Handlers
$("#jobs-per-page").change(function () { // When user changes the results per page dropdown menu
    jobsPerPage = $("#jobs-per-page :selected").val();
    hideExtraRows(1);
    updatePaginationBar(1);
});

$(".paginationLink").click(function () {
    var page = parseInt($(this).text()); // Find what number user clicked
    hideExtraRows(page);
    updatePaginationBar(page);
});

$(".fa").click(function () {
    if ($(this).hasClass("fa-angle-double-left")) { // If they click "go to first page" arrow
        hideExtraRows(1);
        updatePaginationBar(1);
    }
    else if ($(this).hasClass("fa-angle-double-right")) { // If they click "go to last page" arrow
        hideExtraRows(totalPages); 
        updatePaginationBar(totalPages);
    };
});

var filter;
var typedYet = false;
$("#TitleSearch").keyup(function () { // Whenever user types in search bar
    if (typedYet === false) { // If this is their first time typing, make the table appear
        typedYet = true;
        $("#job-listings-table").toggle();
        $("#paginationContainer").toggle();
    };

    filter = document.getElementById("TitleSearch").value.toUpperCase(); // Filter = whatever user has typed in

    // Loop through all table rows, and hide those that don't match the search query
    for (let i = 1; i < rows.length; i++) {
        company = rows[i].getElementsByTagName("td")[1]; // If we change table layout then you must also change the index here
        title = rows[i].getElementsByTagName("td")[4]; // If we change table layout then you must also change the index here

        if (company.innerHTML.toUpperCase().indexOf(filter) > -1) { // Check if company matches
            $(rows[i]).css("display", "table-row");
            $(rows[i]).removeClass("filtered");
        }
        else if (title.innerHTML.toUpperCase().indexOf(filter) > -1) { // Check if job title matches
            $(rows[i]).css("display", "table-row");
            $(rows[i]).removeClass("filtered");
        } else { // Else add this "filtered" class, which exists so that the hideExtraRows() function doesn't interfere
            $(rows[i]).addClass("filtered");
        };
    };
    hideExtraRows(1); // Every time user searches just send them to page 1
    updatePaginationBar(1);
});

var hasBeenFocusedYet = false;
$("#TitleSearch").focus(function () { // Once they click/focus on search bar it will move up to top of page
    if (!hasBeenFocusedYet) {
        hasBeenFocusedYet = true;
        $("#search-wrapper").animate({ height: "35px" }, 750);
    };
});

// Function declarations
function hideExtraRows(page) {
    rowsFiltered = document.querySelectorAll("tr:not(.filtered)");
    for (let i = 1; i <= rowsFiltered.length; i++) {
        if (i > (page - 1) * jobsPerPage && i <= page * jobsPerPage) { // Checks if row is on the current page or not
            $(rowsFiltered[i]).css("display", "table-row");
        } else {
            $(rowsFiltered[i]).css("display", "none");
        };
    };
};

function updatePaginationBar(page) {
    /* Update Pagination Bar Function:
    * Bootstrap makes the last pagination link have rounded edges and makes it difficult to get the same style on other links 
    * That's why the last pagination link must always be visible if the pagination bar is visible
    * You'd have to change the bootstrap files to overwrite this behavior
    * Pagination links that aren't the first or last one can be hidden if they're not necessary
    * Ideally, the middle pagination link gets the "active" class. This is the default because it looks best/makes sense
    * However, if the "active" link is left or right of the middle it goes through a different code block
    * To-the-left, dead center, and to-the-right are all handled in seperate code blocks (only when all links are visible)
    * Code blocks for displaying none, some, and all pagination links are also seperate */

    totalPages = Math.ceil((rowsFiltered.length - 1) / jobsPerPage);

    // Clear the pagination bar
    var currentActivePage = document.getElementsByClassName("active");
    $(".active").removeClass("active");

    if (totalPages <= 1) { // Hide the page links if there's no need for them
        $(pageLinks).css("display", "none");
        $(".fa-2x").css("display", "none");
        $(pageLinks[0]).addClass("active"); // This is mainly here for tableSorter.js so it can find the current page
    }
    else if (totalPages < maxPagesDisplayed) { // Hide some page links but don't hide the last one because it has a nice border-radius
        $(".fa-2x").css("display", "inline-block");

        for (let i = 1; i < maxPagesDisplayed; i++) {
            if (i < totalPages) {
                $(pageLinks[i - 1]).html("<a>" + i + "</a>");
                $(pageLinks[i - 1]).css("display", "inline");
            } else {
                $(pageLinks[i - 1]).css("display", "none");
            };
        };
        var lastPageLink = $(pageLinks[maxPagesDisplayed - 1]);
        $(lastPageLink).html("<a>" + totalPages + "</a>");
        $(lastPageLink).css("display", "inline");
        if (page === totalPages) {
            $(lastPageLink).addClass("active");
        } else {
            $(pageLinks[page - 1]).addClass("active");
        };
    } else { // If all page links are visible do this
        $(pageLinks).css("display", "inline"); // Show all
        $(".fa-2x").css("display", "inline-block");

        // Reassign "active" class
        if (page <= Math.floor(maxPagesDisplayed / 2)) { // If page is left of middle pagiantion link
            for (let i = 1; i <= maxPagesDisplayed; i++) {
                $(pageLinks[i - 1]).html("<a>" + i + "</a>");
            };
            $(pageLinks[page - 1]).addClass("active");
        }
        else if (page > totalPages - Math.floor(maxPagesDisplayed / 2)) { // Else if page is right of middle pagiantion link
            var iterationCount = 0; // Iteration count will be different than var i so it's necessary to keep track of this
            for (let i = totalPages - maxPagesDisplayed + 1; i <= totalPages; i++) {
                $(pageLinks[iterationCount]).html("<a>" + i + "</a>");
                if (page === i) {
                    $(pageLinks[iterationCount]).addClass("active");
                };
                iterationCount++;
            };
        } else { // Else if this is the middle pagination link
            for (let i = 0; i <= maxPagesDisplayed; i++) {
                $(pageLinks[i]).html("<a>" + (page + Math.floor(i + 1 - maxPagesDisplayed / 2)) + "</a>");
            };
            $(pageLinks[Math.floor(maxPagesDisplayed / 2)]).addClass("active"); // Assign "active" class to middle page link
        };
    };
};