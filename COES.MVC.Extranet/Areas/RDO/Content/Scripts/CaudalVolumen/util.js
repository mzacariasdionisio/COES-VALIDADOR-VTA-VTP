function findIndiceMerge(col, lista){
    for(key in lista){
        if ((col >= lista[key].col) && (col < (lista[key].col + lista[key].colspan))) {
            return key;
        }
    }
    return -1;
}