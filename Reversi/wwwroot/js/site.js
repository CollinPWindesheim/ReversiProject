const Game = (function(url){

   const configMap = {
      spel: null,
      size: 8,
      turn: null,
      move: true,
      init: false,
      color: null,
      unread: 0,
      forfeit: false
   };

   const pas = document.getElementById('pas');
   const bord = document.getElementById('bord');
   const list = document.getElementById('list');
   const chat = document.getElementById('chat');
   const spel = document.getElementById('spel');
   const lobby = document.getElementById('lobby');
   const finish = document.getElementById('finish');
   const layout = document.getElementById('layout');
   const unread = document.getElementById('unread');
   const status = document.getElementById('status');
   const loading = document.getElementById('loading');
   //const chatbox = document.getElementById('chat-box');
   //const chatsend = document.getElementById('chat-send');

   const error = err => new Alert({
      message: err.toString(),
      type: 'danger',
      dismissible: true,
      buttons: {
         text: 'Herladen',
         type: 'light',
         callback: function() {
            location.reload();
         }
      },
      animate: true,
      parent: 'main'
   });

   const connection = new signalR.HubConnectionBuilder().withUrl("/ReversiMvcAppHub").build();
   connection.start().catch(function (err) {
      error(err.toString()).show();
   });
   connection.on("Init", function (group) {
      if (configMap.init) {
         setGroup(group);
      } else {
         let g = 'spel';
         if (group == 'lobby') { g = 'lobby'; }

         fetch('/api/' + g).then(response => {
            if (!response.ok) {
               throw new Error('Er is een fout opgetreden. Probeer het later opnieuw.');
            }

            return response.json();
         }).then(function (json) {
            init(g, json);
         }).catch(err => error(err).show());
      }
   });
   connection.on("Group", setGroup);
   connection.on("Update", updateLobby);
   connection.on("Start", function (id) {
      if (configMap.spel == id) {
         connection.invoke("Group").catch(err => error(err.toString()));
      } else {
         updateLobby();
      }
   });
   connection.on("Move", function () {
      updateSpel(showSpel);
   });
   connection.on("Finish", function (data) {
      showSpel(data);
   });
   connection.on("Message", function (message) {
      if (message.spelId == configMap.spel) {
         receiveChat(message);
      }
   });

   function init(init, data) {

      configMap.init = true;

      const ready = function () {
         layout.removeEventListener('animationend', ready);
         layout.classList.remove('fade', 'fade-in', 'show');
         loading.classList.add('d-none');

         if (init == 'spel') {
            if (data.spelers && data.spelers.length == 2) {
               setTimeout(_ => {
                  showSpel(data, data.spel.bord.length > 8 ? 'parallel' : 'serie');
               }, 250);
            } else {
               prepareSpel(false);
            }
         }
      };

      document.getElementById('modal-add-submit').addEventListener('click', function () {
         let description = document.getElementById('modal-add-description');
         let size = document.getElementById('modal-add-size');

         if (description.matches(':invalid') || size.matches(':invalid')) {
            return;
         }

         addSpel(description.value.trim(), parseInt(size.value));
      });
      document.getElementById('settings-toggle').addEventListener('click', function () {
         let settings = document.getElementById('settings');
         let expand = $('#settings-expand');
         if (settings.classList.contains('expanded') && !window.matchMedia('(min-width: 768px)').matches) {
            settings.classList.remove('expanded');
            expand.slideUp(250);
         } else {
            settings.classList.add('expanded');
            configMap.unread = 0;
            expand.slideDown(250, unreadChat);
         }
      });
      document.getElementById('leave').addEventListener('click', leaveSpel);
      document.getElementById('forfeit').addEventListener('click', forfeitSpel);
      pas.querySelector('button').addEventListener('click', pasSpel);
      finish.querySelector('button').addEventListener('click', function () {

         configMap.color = null;
         configMap.turn = null;
         configMap.spel = null;
         configMap.size = 8;

         connection.invoke("Group").catch(err => error(err.toString()));
      });

      chatbox.addEventListener('keypress', function (e) {
         if (e.key == 'Enter' && !e.shiftKey) {
            e.preventDefault();
            sendChat();
            return false;
         }
      });
      chatbox.addEventListener('input', function () {
         chatsend.disabled = !chatbox.value.trim();
      });
      chatsend.addEventListener('click', sendChat);

      $('#list').on('click', 'li.list-group-item-action', function () {
         joinSpel($(this).data('id'));
      });
      $('#bord').on('click', '.tile.move', function () {
         let t = $(this);
         moveSpel(t.index(), t.parent().index());
      });

      if (init == 'lobby') {
         let participating = data.find(l => l.participating);
         if (participating) {
            data = participating;
            init = 'spel';
         } else {
            loadLobby(data);
         }
      }
      if (init == 'spel') {
         loadSpel(data);
      }
      switchLayout(init);

      layout.classList.add('fade-in', 'show');
      layout.addEventListener('animationend', ready);
   };

   function updateLobby() {
      fetch('/api/lobby').then(response => {
         if (!response.ok) {
            throw new Error('Er is een fout opgetreden. Probeer het later opnieuw.');
         }

         return response.json();
      }).then(function (json) {
         loadLobby(json);
         switchLayout(configMap.spel ? 'spel' : 'lobby');
      }).catch(err => error(err).show());
   }
   function loadLobby(data) {
      list.innerHTML = '';

      let lc = document.createElement('div');
      lc.classList.add('last-child', 'text-center');
      lc.innerText = "Er zijn geen spellen waaraan je mee kan doen.";

      list.append(lc);
      data.forEach(item => {

         let li = document.createElement('li');
         li.classList.add('list-group-item', item.participating ? 'text-muted' : 'list-group-item-action', 'flex-column', 'align-items-start');
         li.setAttribute('data-id', item.spel.id);

         let header = document.createElement('div');
         header.classList.add('d-flex', 'w-100', 'justify-content-between');

         let title = document.createElement('h5');
         title.classList.add('mb-1');
         title.innerText = item.speler.name;

         let date = document.createElement('small');
         date.classList.add('text-muted');
         date.innerText = new Date(item.spel.date).toLocaleString('nl-NL');

         header.append(title, date);

         let desc = document.createElement('p');
         desc.classList.add('mb-1');
         desc.innerText = item.spel.description;

         let infoHolder = document.createElement('small');
         infoHolder.classList.add('text-muted', 'd-flex');

         [
            {
               icon: 'border_all',
               info: item.spel.size
            },
            {
               icon: 'emoji_events',
               info: Math.round(item.speler.won / item.speler.sum * 100) + '%'
            }
         ].forEach(i => {
            let iconGroup = document.createElement('div');
            iconGroup.classList.add('d-flex', 'mr-4', 'mr-sm-5');

            let icon = document.createElement('i');
            icon.classList.add('material-icons');
            icon.innerText = i.icon;

            let info = document.createElement('span');
            info.classList.add('mb-auto', 'ml-3', 'mt-auto');
            info.innerText = i.info;

            iconGroup.append(icon, info);
            infoHolder.append(iconGroup);
         });

         li.append(header, desc, infoHolder);
         list.append(li);
      });
   }

   function updateSpel(callback) {
      fetch('/api/spel').then(response => {
         if (!response.ok) {
            throw new Error('Er is een fout opgetreden. Probeer het later opnieuw.');
         }

         return response.json();
      }).then(callback).catch(err => error(err).show());
   }
   function loadSpel(data, start = false) {
      configMap.size = data.spel.size;
      configMap.spel = data.spel.id;
      configMap.unread = 0;

      chat.innerHTML = '';
      chatbox.value = '';
      chatsend.disabled = true;

      bord.style.setProperty('--tile-size', 'calc(100% / ' + configMap.size + ')');
      bord.innerHTML = '';

      finish.classList.add('d-none');
      finish.classList.remove('show', 'fade-in');

      for (let y = 0; y < data.spel.size; y++) {
         let row = document.createElement('div');
         row.classList.add('row');
         for (let x = 0; x < data.spel.size; x++) {
            let tile = document.createElement('div');
            tile.classList.add('tile');
            row.append(tile);
         }
         bord.append(row);
      }

      let prepare = data.spelers && data.spelers.length == 2;
      prepareSpel(prepare);

      if (start && prepare) {
         showSpel(data, data.spel.bord.length > 8 ? 'parallel' : 'serie');
      }
   }
   function showSpel(data, forceSort = false) {
      if (data.color) { configMap.color = data.color; }
      configMap.turn = data.spel.turn;

      prepareSpel(true);

      const turn = document.getElementById('turn');
      if (configMap.turn == 1) {
         turn.classList.add('black');
         turn.classList.remove('white');
      } else if (configMap.turn == 2) {
         turn.classList.add('white');
         turn.classList.remove('black');
      }

      status.innerText = (configMap.turn == configMap.color ? 'Jij bent' : 'Je tegenstander is') + ' aan de beurt';

      let scoreBlack = data.spel.bord.filter(c => c.kleur == 1).length;
      let scoreWhite = data.spel.bord.filter(c => c.kleur == 2).length;
      for (let seBlack of document.getElementsByClassName('score-black')) { seBlack.innerText = scoreBlack; }
      for (let seWhite of document.getElementsByClassName('score-white')) { seWhite.innerText = scoreWhite; }
      data.spelers.forEach(s => document.getElementById('name-' + (s.color == 1 ? 'black' : 'white')).innerText = s.name);

      setMoves();
      let sort = forceSort || data.spel.history.pop() || true;
      showFiches(data.spel.bord, sort).then(function () {
         setMoves(data.spel.moves);

         configMap.forfeit = false;
         if (data.spel.state > 1) {

            const finishResult = document.getElementById('finish-result');

            let finishText;
            if (data.spel.state == 2) {
               finishResult.classList.remove('d-none');
               if (scoreBlack == scoreWhite) {
                  finishText = 'Gelijk spel';
               } else {
                  if ((scoreBlack > scoreWhite && configMap.color == 1) || (scoreBlack < scoreWhite && configMap.color == 2)) {
                     finishText = 'Gefeliciteerd!';
                  } else {
                     finishText = 'Helaas...';
                  }
               }
            } else {
               finishResult.classList.add('d-none');
               if ((data.spel.state == 3 && configMap.color == 1) || (data.spel.state == 4 && configMap.color == 2)) {
                  finishText = 'Je hebt opgegeven';
               } else {
                  finishText = 'Je tegenstander heeft opgegeven';
               }
            }
            document.getElementById('finish-text').innerText = finishText;

            finish.classList.remove('d-none');
            finish.classList.add('show', 'fade-in');
            configMap.forfeit = true;
         } else if (data.color == data.spel.turn && !data.spel.moves.length) {
            pas.classList.remove('d-none');
            pas.classList.add('show', 'fade-in');
         }

         for (let f of document.getElementsByClassName('forfeit')) {
            f.disabled = configMap.forfeit;
         }
      });

   }
   function prepareSpel(ready = false) {
      let waiting = $('#settings-waiting');
      let playing = $('#settings-playing');

      if (ready) {
         waiting.slideUp(250, function () {
            playing.slideDown(250);
         });
      } else {
         status.innerText = 'Aan het wachten...';
         playing.slideUp(250, function () {
            waiting.slideDown(250);
         });
      }
   }
   function addSpel(description, size) {
      fetch('/api/lobby', {
         method: 'POST',
         headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
         },
         body: JSON.stringify({
            Description: description,
            Size: size
         })
      }).then(response => {
         if (!response.ok) {
            configMap.spel = null;
            throw new Error('Er is een fout opgetreden. Probeer het later opnieuw.');
         }

         return response.json();
      }).then(json => {
         loadSpel(json);
         switchLayout('spel');
         $('#modal-add').modal('hide');
      }).catch(err => error(err).show());
   }
   function joinSpel(id) {
      configMap.spel = id;
      fetch('/api/lobby/' + id, {
         method: 'PUT',
         headers: {
            'Accept': 'application/json'
         }
      }).then(response => {
         if (!response.ok) {
            configMap.spel = null;
            throw new Error('Er is een fout opgetreden. Probeer het later opnieuw.');
         }
      }).catch(err => error(err).show());
   }
   function leaveSpel() {
      configMap.spel = null;
      fetch('/api/lobby', {
         method: 'DELETE'
      }).then(response => {
         if (!response.ok) {
            throw new Error('Er is een fout opgetreden. Probeer het later opnieuw.');
         }

         return response.json();
      }).catch(err => error(err).show());
   }
   function forfeitSpel() {
      if (!configMap.forfeit) {
         configMap.forfeit = true;
         fetch('/api/spel', {
            method: 'DELETE'
         }).then(response => {
            if (!response.ok) {
               configMap.forfeit = false;
               throw new Error('Er is een fout opgetreden. Probeer het later opnieuw.');
            }
         }).catch(err => error(err).show());
      }
   }
   function pasSpel() {
      fetch('/api/spel', {
         method: 'PUT'
      }).then(response => {
         if (!response.ok) {
            throw new Error('Er is een fout opgetreden. Probeer het later opnieuw.');
         }

         pas.classList.add('d-none');
         pas.classList.remove('show', 'fade-in');
      }).catch(err => error(err).show());
   }
   function moveSpel(x, y) {
      if (configMap.color == configMap.turn && configMap.move) {
         configMap.move = false;
         fetch('/api/spel', {
            method: 'POST',
            headers: {
               'Accept': 'application/json',
               'Content-Type': 'application/json'
            },
            body: JSON.stringify({
               X: x,
               Y: y
            })
         }).then(response => {
            configMap.move = true;
            if (!response.ok) {
               throw new Error('Deze zet is ongeldig.');
            }
         }).catch(err => error(err).show());
      }
   }

   function sendChat() {
      let msg = chatbox.value.trim();
      if (msg) {
         fetch('/api/message/' + configMap.spel, {
            method: 'POST',
            headers: {
               'Content-Type': 'application/json'
            },
            body: JSON.stringify(msg)
         }).then(response => {
            if (!response.ok) {
               throw new Error('Kan je bericht niet versturen...');
            }

            chatbox.value = '';
         }).catch(err => error(err).show());
      }
   }
   function receiveChat(message) {
      if (!message.type) {
         let msgEl = document.createElement('div');
         msgEl.classList.add('chat-bubble-msg');
         msgEl.innerText = message.content;

         let lastChat = chat.lastElementChild;
         if (lastChat && ((message.color == configMap.color && lastChat.classList.contains('chat-bubble-right')) || (message.color != configMap.color && lastChat.classList.contains('chat-bubble-left')))) {
            lastChat.lastElementChild.append(msgEl);
         } else {
            let bubbleEl = document.createElement('div');
            bubbleEl.classList.add('chat-bubble', 'chat-bubble-' + (message.color == configMap.color ? 'right' : 'left'));

            let bodyEl = document.createElement('div');
            bodyEl.classList.add('chat-bubble-body');

            bodyEl.append(msgEl);
            bubbleEl.append(bodyEl);
            chat.append(bubbleEl);
         }

         if (configMap.color != message.color && chat.offsetParent === null) {
            configMap.unread++;
            unreadChat();
         }
      // } else if (message.type == 1) {
      //
      // } else if (message.type == 2) {
      //
      }

      chat.scrollTo(0, chat.scrollHeight);
   }
   function unreadChat() {
      unread.classList.toggle('d-none', configMap.unread <= 0);
      unread.innerText = configMap.unread;
   }

   function setGroup(group) {
      if (group == 'lobby') {
         updateLobby();
      } else {
         updateSpel(function (json) {
            loadSpel(json, true);
            switchLayout(configMap.spel ? 'spel' : 'lobby');
         });
      }
   }

   // Helper functies
   const withinBoundries = (x, y) => !(x < 0 || x >= configMap.size || y < 0 || y >= configMap.size);
   const switchLayout = function (to) {
      let s = to != 'lobby' && to;
      spel.classList.toggle('d-none', !s);
      lobby.classList.toggle('d-none', s);
   }

   const showFiche = (color, x, y) => {
      if (x === undefined && y === undefined && typeof color === 'object') {
         x = color.x;
         y = color.y;
         color = color.color || color.kleur;
      }

      if (!withinBoundries(x, y)) {
         console.error('Given coordinates are not valid.');
         return new Promise((resolve, reject) => reject('Given coordinates are not valid.'));
      }

      if (color == 1 || (color + "").toLowerCase() == 'zwart') {
         color = 'black';
      } else if (color == 2 || (color + "").toLowerCase() == 'wit') {
         color = 'white';
      }

      const tile = bord.querySelector('.row:nth-child(' + (y + 1) + ')>.tile:nth-child(' + (x + 1) + ')');
      let fiche = tile.querySelector('.fiche');

      let noAnimation = false;
      if (fiche) {
         if (fiche.classList.contains(color.toLowerCase())) {
            noAnimation = true;
         }
         fiche.classList.remove('black', 'white');
      } else {
         fiche = document.createElement('div');
         tile.append(fiche);
      }

      fiche.classList.add('fiche');
      if (color == 'black' || color == 'white') {
         fiche.classList.add(color.toLowerCase());
      }

      const finishAnimation = function (resolve) {
         fiche.removeEventListener('transitionend', finishAnimation);
         fiche.removeEventListener('animationend', finishAnimation);
         resolve(fiche);
      }

      return new Promise(resolve => {
         if (noAnimation) {
            setTimeout(_ => resolve(), 250);
         } else {
            fiche.addEventListener('transitionend', function () {
               finishAnimation(resolve);
            });
            fiche.addEventListener('animationend', function () {
               finishAnimation(resolve);
            });
         }
      });
   }
   const showFiches = (fiches, order = false, reset = false) => {

      if (reset) {
         for (var x = 0; x < configMap.size; x++) {
            for (var y = 0; y < configMap.size; y++) {
               let fiche = fiches.find(f => f.x == x && f.y == y);
               if (!fiche) {
                  showFiche('', x, y);
               }
            }
         }
      }

      const run = resolve => {
         if (!fiches.length) {
            resolve();
            return;
         }

         if (typeof order === 'object' && Number.isInteger(order.x) && Number.isInteger(order.y)) {
            if (!order.layer) {
               fiches = fiches.filter(f => withinBoundries(f.x, f.y));
               order.layer = 0;
            }

            let currentFiches = [];
            for (var x = order.x - order.layer; x <= order.x + order.layer; x++) {
               for (var y = order.y - order.layer; y <= order.y + order.layer; y++) {
                  if ((x == order.x - order.layer || x == order.x + order.layer || y == order.y - order.layer || y == order.y + order.layer) && withinBoundries(x, y)) {
                     fiches = fiches.filter(f => {
                        if (f.x == x && f.y == y) {
                           currentFiches.push(f);
                           return false;
                        }

                        return true;
                     });
                  }
               }
            }

            order.layer++;
            if (currentFiches.length) {
               let wait = true;
               currentFiches.forEach(f => showFiche(f).then(_ => {
                  if (wait) {
                     wait = false;
                     run(resolve);
                  }
               }));
            } else {
               setTimeout(_ => run(resolve), 250);
            }
         } else if (order === true || order == 'serie') {
            showFiche(fiches.shift()).then(_ => run(resolve));
         } else if (order === false || order == 'parallel') {
            fiches.forEach(f => showFiche(f).then(_ => resolve()));
         }
      }

      return new Promise(resolve => {
         run(resolve);
      });
   }
   const setMoves = moves => {
      for (let tile of document.getElementsByClassName('tile') || []) { tile.classList.remove('move'); }
      if (moves) {
         if (!Array.isArray(moves)) { moves = [moves]; }
         moves.forEach(m => document.querySelector('.row:nth-child(' + (m.y + 1) + ')>.tile:nth-child(' + (m.x + 1) + ')').classList.add('move'));
      }
   }
})();

const Alert = function (params) {

   const typeList = ["primary", "secondary", "success", "danger", "warning", "info", "light", "dark"];

   let msg;
   let t;
   let p;
   let close = false;
   let animation = false;
   let actionButtons = [];

   const alert = document.createElement("div");

   const contentHolder = document.createElement("div");
   const content = document.createElement("p");
   const iconHolder = document.createElement("i");

   iconHolder.classList.add("col-auto", "material-icons", "d-none", "d-sm-inline-block");
   contentHolder.classList.add("row");
   content.classList.add("col");

   alert.classList.add('alert');
   contentHolder.append(content);
   alert.append(contentHolder);

   const buttonDiv = document.createElement("div");
   buttonDiv.classList.add("btns", "text-right");
   alert.append(buttonDiv);

   const closeButton = document.createElement("button");
   const closeSpan   = document.createElement("span");

   closeSpan.innerHTML = "&times;";
   closeButton.append(closeSpan);
   closeButton.classList.add('close');
   closeButton.setAttribute('type', 'button');
   closeButton.setAttribute('aria-label', 'Close');
   closeButton.addEventListener('click', hide);
   // closeButton.setAttribute('data-dismiss', 'alert');

   /**
    * Gets / sets the text of the message
    *
    * @param string text Content of the message
    *
    */
   function message(text) {
      if (text === undefined) {
         return msg;
      }

      msg = text;
      content.innerText = msg;
   }

   /**
    * Gets / sets if the type should be
    *
    * @param string successOrError 'success' or 'error'
    * @param bool successOrError success = true and error = false
    *
    */
   function type(alertType) {
      if (alertType === undefined) {
         return t;
      }

      let reset = true;
      typeList.forEach(typeClass => {
         let toggle = alertType == typeClass;
         alert.classList.toggle('alert-' + typeClass, toggle);

         if (toggle) {
            t = typeClass;
            reset = false;
         }
      });

      if (reset) {
         t = '';
      }
   }

   /**
    * Gets / sets the parentelement to append the alert to
    *
    * @param HTMLElement element
    *
    */
   function parent(element) {
      if (element === undefined) {
         return p;
      }

      if (element instanceof HTMLElement) {
         p = element;
      } else if (typeof element === 'string') {
         p = document.querySelector(element);
      } else {
         p = document.body;
      }
   }

   /**
    * Show / hide a close button
    *
    * @param bool force a state, leave empty to toggle
    *
    */
   function dismissible(force = null) {
      if (force === null) {
         force = !close;
      }

      force = !!force;
      if (force != close) {
         alert.classList.toggle('alert-dismissible', force);
         if (force) {
            alert.append(closeButton);
         } else {
            closeButton.remove();
         }
      }
   }

   /**
    * Set all buttons or one button
    *
    * @param Array buttons Array van buttons met een text, callback, close en type.
    * @param Object buttons
    *
    */
   function button(buttons) {
      actionButtons.forEach(item => {
         item.remove();
      });

      actionButtons = [];

      if (buttons) {
         if (!Array.isArray(buttons)) {
            buttons = [buttons];
         }

         buttons.forEach(item => {
            if (typeof item === 'object') {
               let tempButton = document.createElement("button");
               tempButton.setAttribute('type', 'button');
               tempButton.classList.add('btn');

               tempButton.innerText = item.text;
               if (typeof item.callback === 'function') {
                  tempButton.addEventListener('click', item.callback);
               }
               if (item.close) {
                  tempButton.addEventListener('click', hide);
               }
               if (typeList.indexOf(item.type) >= 0) {
                  tempButton.classList.add('btn-' + item.type);
               }

               buttonDiv.append(tempButton);

               actionButtons.push(tempButton);
            }
         });
      }
   }

   /**
    * Sets the icon
    *
    * @param string i name of the icon
    *
    */
   function icon(i) {
      if (i) {
         iconHolder.innerText = i;
         contentHolder.prepend(iconHolder);
      } else {
         iconHolder.remove();
      }
   }

   /**
    * Toggle animationstate
    *
    * @param bool a state
    *
    */
   function animate(a) {
      animation = !!a;
   }

   /**
    * Shake the alert
    */
   function shake() {
      if (document.contains(alert) && !alert.classList.contains('fade-out')) {
         alert.classList.add('shake');
      }
   }

   /**
    * Show the message
    *
    * @param int ms Amount of ms to show the alert. Does not hide when ms < 1.
    *
    */
   function show(ms = 0) {
      alert.classList.toggle('fade-in', animation);
      p.prepend(alert);

      let h = JSON.parse(localStorage.getItem('Alert'));
      if (!h || !h.length) {
         h = [];
      }

      h.push({
         message: msg,
         type: t
      });

      localStorage.setItem('Alert', JSON.stringify(h));

      if (ms > 0) {
         setTimeout(hide, ms);
      }
   }

   /**
    * Hide the alert
    */
   function hide() {
      alert.classList.remove('fade-in');

      if (document.contains(alert)) {
         if (animation) {
            alert.classList.add('fade-out');
         } else {
            alert.remove();
         }
      }
   }

   alert.addEventListener('animationend', function () {
      if (alert.classList.contains('fade-out')) {
         alert.classList.remove('fade-out');
         animation && alert.remove();
      }

      alert.classList.remove('fade-in', 'shake');
   });

   if (typeof params !== 'object') {
      params = {};
   }

   type(params.type || 'danger');
   parent(params.parent || document.body);
   message(params.message || '');
   dismissible(params.dismissible || false);
   button(params.buttons || []);
   icon(params.icon || '');
   animate(params.animate);

   if (params.shake) {
      setTimeout(shake, Number.isInteger(params.shake) && params.shake > 0 ? params.shake : 3000);
   }

   return {
      show: show,
      hide: hide,
      type: type,
      parent: parent,
      message: message,
      dismissible: dismissible,
      buttons: button,
      icon: icon,
      animate: animate,
      shake: shake,

      history: function () {
         let h = JSON.parse(localStorage.getItem('Alert'));
         if (h && h.length) {
            console.log(h.map(m => "<type " + m.type + "> - <" + m.message + ">").join("\n"));
         }
      },
      reset: function () {
         localStorage.setItem('Alert', '[]');
      }
   };
}
