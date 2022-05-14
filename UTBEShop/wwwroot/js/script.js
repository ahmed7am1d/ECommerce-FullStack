//#region 1- Typing Effect + Button Visability

var myText = `A proud student, a graduate or just a University supporter? And you have already bought that cool hoodie. What about adding some original clothing items designed by our students to your collection? Go to our official showroom be.utb.cz`;
i =0 ;
//when the content of the page load 
document.addEventListener('DOMContentLoaded',function(){

'use strict';
var typeWriter = setInterval(function(){
    document.getElementById('descriptionShop').textContent += myText[i];
    i+=1;
    if(i > myText.length -1) {
        //make button visit website visiable 
        document.getElementById('buttonVisit').style.display ="block";
        clearInterval(typeWriter);
    }

},30)



},false)



//#endregion 


//#region Options for login class
//1- Get the user div and Admin div inside varaibles 
const userDiv = document.querySelector('.user');
const adminDiv = document.querySelector('.admin');

//2-Event listern for both element 
userDiv.addEventListener('click', (event) => {
    //--check if admin div has the class active 
    //--if yes remove it and add it to user div 
    if (adminDiv.classList.contains('active')) {
        adminDiv.classList.remove('active')
        userDiv.classList.add('active');
    }
});

adminDiv.addEventListener('click', (event) => {
    if (userDiv.classList.contains('active')) {
        userDiv.classList.remove('active')
        adminDiv.classList.add('active');
    }
})

//#endregion
