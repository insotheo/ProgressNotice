const links = document.getElementsByTagName("a");

for(let i = 0; i < links.length; i++){
    links[i].setAttribute("target", "_blank");
}