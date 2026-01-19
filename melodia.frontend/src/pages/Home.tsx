import { useState } from 'react';
import './Home.css';
import logo from '../assets/images/logo.png';
import background from '../assets/images/background.png';

function Home() {
  const [soundName, setSoundName] = useState('');
  const [description, setDescription] = useState('');
  const [duration, setDuration] = useState(5);
  const [isCreating, setIsCreating] = useState(false);
  const [musicCreated, setMusicCreated] = useState(false);
  const [musicUrl, setMusicUrl] = useState('');

  const handleCreate = async () => {
    setIsCreating(true);
    setMusicCreated(false);
    
    try {
      // Implementar chamada à API
      const response = await fetch('/api/create-sound', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ soundName, description, duration })
      });
      
      if (response.ok) {
        const data = await response.json();
        setMusicUrl(data.url);
        setMusicCreated(true);
      }
    } catch (error) {
      console.error('Erro ao criar música:', error);
    } finally {
      setIsCreating(false);
    }
  };

  const handleDownload = () => {
    if (musicUrl) {
      const link = document.createElement('a');
      link.href = musicUrl;
      link.download = `${soundName || 'musica'}.mp3`;
      link.click();
    }
  };

  return (
    <div className="home-container" style={{ backgroundImage: `url(${background})` }}>
      <div className="form-section">
        <div className="form-header">
          <h1>Criar Som</h1>
          <img src={logo} alt="Melodia Logo" className="header-logo" />
        </div>
        
        <div className="form-group">
          <label htmlFor="soundName">Nome do Som</label>
          <input
            type="text"
            id="soundName"
            className="input-field"
            placeholder="Digite o nome do som"
            value={soundName}
            onChange={(e) => setSoundName(e.target.value)}
          />
        </div>

        <div className="form-group">
          <label htmlFor="description">Descrição</label>
          <textarea
            id="description"
            className="textarea-field"
            placeholder="Descreva como deseja que seja feito o som"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            rows={5}
          />
        </div>

        <div className="form-group">
          <label>Duração (segundos)</label>
          <div className="duration-options">
            {[5, 10, 15, 20, 25, 30].map((seconds) => (
              <label key={seconds} className="checkbox-label">
                <input
                  type="radio"
                  name="duration"
                  value={seconds}
                  checked={duration === seconds}
                  onChange={() => setDuration(seconds)}
                />
                <span>{seconds}s</span>
              </label>
            ))}
          </div>
        </div>

        <div className="button-group">
          <button 
            className="btn btn-create" 
            onClick={handleCreate}
            disabled={isCreating || !soundName || !description}
          >
            <svg className="btn-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <path d="M12 5v14M5 12h14" />
            </svg>
            {isCreating ? 'Criando...' : 'Criar'}
          </button>
          <button 
            className="btn btn-download" 
            onClick={handleDownload}
            disabled={!musicCreated}
          >
            <svg className="btn-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4" />
              <polyline points="7 10 12 15 17 10" />
              <line x1="12" y1="15" x2="12" y2="3" />
            </svg>
            Download
          </button>
        </div>
      </div>
    </div>
  );
}

export default Home;
