import React from 'react';
import getAxios from '../AuthAxios';

const Joke = ({joke}) => {
    
    const{punchLine,setUp,id, likes, dislikes}=joke;

  
    return <div>
    <div className="card card-body bg-light mb-3">
        <h5>{setUp}</h5>
        <h5>{punchLine}</h5>
        <span>Likes: {likes}</span>
        <br/>
        <span>Dislikes: {dislikes}</span>
        </div>
</div>
}

export default Joke;