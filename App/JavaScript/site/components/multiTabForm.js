export function InitMutiTabForm(){

    document.querySelectorAll('.nav-link').forEach(function (tab) {
        tab.addEventListener('click', function (e) {
            document.querySelectorAll('.tab-pane').forEach(function (pane) {
                pane.classList.remove('show', 'active');
            });
            document.querySelectorAll('.nav-link').forEach((tab => {
                tab.classList.remove('active');
            }))
            document.querySelector(`#${this.dataset.tab}`).classList.add('show', 'active');
            e.target.classList.add('active');
        });
    });
    
    const colorInput = document.getElementById('favoriteColor');
    const foodInput = document.getElementById('favoriteFood');
    
    function handleColorSuggestionClick(event) {
        let target = event.target;
        
        //Traverse up to find the parent span with class 'suggestion'
        while (target && !target.classList.contains('suggestion')) {
            target = target.parentElement;
        }
        
        if (target) {
            //Perform your actions here
            console.log('Color Suggestion span clicked:', target.innerText);
            colorInput.value = target.innerText;
        }
    }
    
    function handleFoodSuggestionClick(event) {
        let target = event.target;
        
        //Traverse up to find the parent span with class 'suggestion'
        while (target && !target.classList.contains('suggestion')) {
            target = target.parentElement;
        }
        
        if (target) {
            //Perform your actions here
            console.log('Color Suggestion span clicked:', target.innerText);
            foodInput.value = target.innerText;
        }
    }
    
    const colorSuggestionDiv = document.getElementById('resultsColor');
    colorSuggestionDiv.addEventListener('click', handleColorSuggestionClick);
    
    const foodSuggestionDiv = document.getElementById('resultsFood');
    foodSuggestionDiv.addEventListener('click', handleFoodSuggestionClick);
}