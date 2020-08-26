var loadingMessages = [
    'Reticulating splines.',
    'Generating witty dialog.',
    'Swapping time and space.',
    'Would you prefer chicken, steak, or tofu?',
    'Would you like fries with that?',
    'The bits are flowing slowly today.',
    'My other loading screen is much faster.',
    '(Insert Quarter)',
    'Do not run! We are your friends!',
    'Do you come here often?',
    'Warning: Don\'t set yourself on fire.',
    'Creating time-loop inversion field.',
    'Spinning the wheel of fortune.',
    'Loading the enchanted bunny.',
    'Computing chance of success.',
    'I\'m sorry Dave, I can\'t do that.',
    'I swear it\'s almost done.',
    'Unicorns are at the end of this road, I promise.',
    'Putting the icing on the cake. The cake is not a lie.',
    'We need more dilithium crystals.',
    'Where did all the internets go?',
    'Spinning the hamster.',
    '99 bottles of beer on the wall...',
    'How did you get here?',
    'Wait, do you smell something burning?',
    'Computing the secret to life, the universe, and everything.',
    'Adults are just kids with money.',
    'I think I am, therefore, I am. I think.',
    'May the forks be with you.',
    'Well, this is embarrassing.',
    'What is the airspeed velocity of an unladen swallow?',
    'Dividing by zero.',
    'If I\'m not back in five minutes, just wait longer.',
    'We\'re going to need a bigger boat.',
    'Entangling superstrings.',
    'Twiddling thumbs.',
    'Trying to sort in O(n).',
    'A different error message? Finally, some progress!',
    'Microwaving coffee.',
    'Winter is coming.',
    'Toss a coin to your witcher.',
    'Distracted by cat gifs.',
    'Let\'s hope it\'s worth the wait.',
    'Ordering 1s and 0s.',
    'Whatever you do, don\'t look behind you.',
    'Please wait... Consulting the manual.',
    'Feel free to spin in your chair.',
    'What the what?',
    'What\'s under there?',
    'Help, I\'m trapped in a loader!',
    'What is the difference btwn a hippo and a zippo? One is really heavy, the other is a little lighter.',
    'Please wait, while we purge the Decepticons for you. Yes, You can thanks us later!',
    'Mining some bitcoins.',
    'When was the last time you dusted around here?',
    'When nothing is going right, go left!',
    'Unix is user-friendly. It\'s just very selective about who its friends are.',
    'Shovelling coal into the server',
    'Pushing pixels.',
    'How about this weather, eh?',
    'Everything in this universe is either a potato or not a potato',
    'Reading Terms and Conditions for you.',
    'Live long and prosper.',
    'Running with scissors.',
    'Definitely not a virus.',
    'You may call me Steve.',
    'You seem like a nice person.',
    'Coffee at my place, tommorow at 10A.M. - don\'t be late!',
    'Work, work.',
    'Patience! This is difficult, you know.',
    'Discovering new ways of making you wait.',
    'Time flies like an arrow; fruit flies like a banana.',
    'Sorry, we\'re busy catching em\' all, we\'ll be done soon.',
    'TODO: Insert elevator music.',
    'Still faster than Windows update.'
];

var loadingContainer = document.getElementById('loadingContainer');
var loadingIcon = document.getElementById('loadingIcon');
var loadingMessage = document.getElementById('loadingMessage');
function activateLoader() {
    setLoadingMessage();
    loadingContainer.classList.add("active");
}
function deactivateLoader() {
    loadingContainer.classList.remove("active");
    loadingIcon.innerText = "🧘‍♀";
}
function setloadingIcon(icon) {
    loadingIcon.innerText = icon;
}
function setLoadingMessage() {
    var index = Math.floor(Math.random() * Math.floor(loadingMessages.length - 1));
    loadingMessage.innerText = loadingMessages[index];
}