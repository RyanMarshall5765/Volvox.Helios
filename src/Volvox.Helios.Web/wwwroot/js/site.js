﻿(function($) {

  $.fn.populateGuildChannels = function(guildId) {
    return this.each(function() {
      const selectElement = $(this);

      selectElement.empty();

      selectElement.append('<option selected="true" disabled hidden>Select a channel</option>');
      selectElement.prop("selectedIndex", 0);
      selectElement.prop("disabled", false);

      const url = `/api/GetGuildChannels?guildId=${guildId}`;

      $.getJSON(url, function (data) {
        $.each(data, function (key, entry) {
          selectElement.append($("<option></option>").attr("value", entry.id).text(entry.name));
        });
      });
    });
  };

  $.fn.populateAnalytics = function () {
    return this.each(function () {
      const element = $(this);

      const url = "/api/GetUserAdminGuilds?inGuild=true";

      $.getJSON(url, function (data) {
        element.empty();

        $.each(data, function (key, entry) {
          element.append(generateGuildDropdownItem(entry.id, entry.name, entry.icon, `/Analytics/${entry.id}`));
        });
      });
    });
  };

  $.fn.populateSettings = function () {
    return this.each(function () {
      const element = $(this);

      const url = "/api/GetUserAdminGuilds";

      $.getJSON(url, function (data) {
        element.empty();

        $.each(data, function (key, entry) {
          element.append(generateGuildDropdownItem(entry.id, entry.name, entry.icon, `/Settings/${entry.id}`));
        });
      });
    });
  };

  function generateGuildDropdownItem(guildId, guildName, guildIcon, href) {
    // guildIcon will be null if the guild doesn't have an icon. Therefore set src to default error icon.
    const noIconUrl = getCurrentTheme() === "true" ? "/images/small/volvox-logo-light.png" : "/images/small/volvox-logo.png";
    const iconUrl = guildIcon === null ? noIconUrl : `https://cdn.discordapp.com/icons/${guildId}/${guildIcon}.png`;

    return (`
      <div>
        <a class="dropdown-item dropdown-item-container" href="${href}">
            <span>
                <img class="dropdown-image-small" src="${iconUrl}">
            </span>
            <span>
                ${guildName}
            </span>
        </a>
    </div>
    `);
  }

  // Material checkbox toggle
  $('.module-enabler input:not(".active")').click((e) => {
    $(e.currentTarget).parent().parent().find('.active').toggleClass('active disabled');
    $(e.currentTarget).parent().toggleClass('active disabled');
  });

  // Sliding border animation
  $('[data-animation="sliding-border"]').click(function () {
    $(this).closest('.sliding-border').toggleClass('animate');
  }, function () {
    $(this).closest('.sliding-border').toggleClass('animate');
    });

  // Dropdown click events
  $('#settingsNavDropdown').click(() => {
    $('#settingsDropDown').populateSettings();
  });
  $('#analyticsNavDropdown').click(() => {
    $('#analyticsDropDown').populateAnalytics();
  });

  // Dark theme
  var darkTheme = window.localStorage.getItem('darkTheme');
  if (!darkTheme) darkTheme = "true";
  toggleTheme(darkTheme);

  // Dark theme toggle
  $('#themeToggler').click((e) => {
    var nextEl = $(e.currentTarget).find('i.d-none').removeClass('d-none').next();
    if (nextEl.length === 0)
      nextEl = $(e.currentTarget).find('i').first();
    nextEl.addClass('d-none');

    darkTheme = window.localStorage.getItem('darkTheme');
    toggleTheme(darkTheme === "true" ? "false" : "true");
  });

  // Toggle theme function
  function toggleTheme(darkThemeEnabled) {
    if (darkThemeEnabled === "true") {
      $('#lightThemeLink').remove();
      $('head').append('<link rel="stylesheet" href="/css/dark-theme.css" id="darkThemeLink">');
      $('.navbar').removeClass('navbar-light bg-light').addClass('navbar-dark');
      $('#themeToggler').find('.fa-sun').removeClass('d-none').parent().find('.fa-moon').addClass('d-none');
      $('#navLogo').attr('src', '/images/small/volvox-logo-light.png');
    } else if (darkThemeEnabled === "false") {
      $('#darkThemeLink').remove();
      $('head').append('<link rel="stylesheet" href="/css/light-theme.css" id="lightThemeLink">');
      $('.navbar').removeClass('navbar-dark').addClass('navbar-light bg-light');
      $('#themeToggler').find('.fa-moon').removeClass('d-none').parent().find('.fa-sun').addClass('d-none');
      $('#navLogo').attr('src', '/images/small/volvox-logo.png');
    }
    window.localStorage.setItem("darkTheme", darkThemeEnabled);
  }

  // Gets the current UI theme
  function getCurrentTheme() {
    return window.localStorage.getItem('darkTheme');
  }

}(jQuery));
