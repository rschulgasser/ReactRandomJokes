import React,{useState,useEffect } from 'react';
import { Link} from 'react-router-dom';
import { useAuthContext } from '../AuthContext';
import getAxios from '../AuthAxios';

const Home = () => {
    const { user } = useAuthContext();
    const[joke, setJoke]=useState('');
   const[like,setLike]=useState(0);
   const[dislike, setDislike]=useState(0);
   const[id, setId]=useState(0);
   const[userJokeLikes, setUserJokeLikes]=useState();
  

    const{punchLine,setUp}=joke;
  
    const getAJoke = async () => {

        const { data } = await getAxios().get('/api/jokes/getrandomjoke');
      
         setJoke(data);
         getLikes(data.id);
         setId(data.id)

    }
    useEffect(() => {

        getAJoke();
        
      }, []);

      const getLikes = async () => {
     
        const { data } = await getAxios().get(`/api/jokes/getlikes?id=${id}`);
     
         setDislike(data.dislike);
         setLike(data.like);
    }

    const interval = setInterval(async() => {
        if(id){
      await getLikes();
        }
      }, 500);

      const AddUpdateLike = async (l) => {

        setUserJokeLikes({jokeId:id, liked: l})
        const { data } = await getAxios().post(`/api/jokes/addupdatelike`, {...userJokeLikes, jokeId:id, liked: l});
        setUserJokeLikes(data);
        await getLikes();
    
    }
  
 
    return <div className="container">
        <div className="row">

            <div className="col-md-6 offset-md-3 card card-body bg-light">
                <div>
                 <h4>{setUp}</h4>
                 <h4>{punchLine}</h4>
                   
                <div>
               {!user&& <div>
                    <Link to="/login">Login to your account to like/dislike this joke</Link>
                </div>}
                <br/>
                {user&&<div>
                    <button onClick={()=>AddUpdateLike(true)} disabled={userJokeLikes && !!userJokeLikes.liked} className="btn btn-primary">Like</button>
                    <button onClick={()=>AddUpdateLike(false)} disabled={userJokeLikes &&!userJokeLikes.liked} className="btn btn-danger">Dislike</button>
                </div>}

                <h4>Likes: {like}</h4>
                <h4>Dislikes: {dislike}</h4>
                <h4>
                    <button onClick={()=>window.location.reload(false)}className="btn btn-link">Refresh</button>
                </h4>
                </div>
            </div>
        </div>
        </div>
        </div>
}

export default Home;