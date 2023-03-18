window.addEventListener('mousemove', (ev) => {
    document.querySelectorAll('.movement-follow-mouse').forEach((el) => el.setAttribute('style', `transform: scale(1.01) translate(${(ev.x - window.innerWidth / 2) / window.innerWidth * 10}px,${(ev.y - window.innerHeight / 2) / window.innerHeight * 10}px)`))
});
window.addEventListener('mouseout', () => {
    document.querySelectorAll('.movement-follow-mouse').forEach((el) => el.setAttribute('style', 'transform: scale(1.005)'));
});
window.addEventListener('mouseenter', () => {
    document.querySelectorAll('.movement-follow-mouse').forEach((el) => el.setAttribute('style', 'transform: scale(1.01)'));
});